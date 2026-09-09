using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class FinalizarSubastaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public FinalizarSubastaCommandHandler(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
        }

        public async Task<bool> HandleAsync(int subastaId)
        {
            //obtenemos el id de la subasta
            var subasta = await _subastaRepository.GetByIdAsync(subastaId);
            if (subasta == null)
                throw new NotFoundException($"La subasta con ID {subastaId} no existe.");

            //verificamos que este activa 
            if (subasta.estado != EstadoSubasta.ACTIVA)
                throw new ValidationException("Solo se pueden finalizar subastas que se encuentren activas.");

            var pujaGanadora = subasta.Pujas?.OrderByDescending(p => p.monto).FirstOrDefault();

            //verificamos que la puja no sea nula
            if (pujaGanadora != null)
            {
                // A. Consolidar débito en el Comprador Ganador (pasa de saldo_retenido a cobro definitivo)
                var billeteraComprador = await _billeteraRepository.GetByUsuarioIdAsync(pujaGanadora.comprador_id);
                if (billeteraComprador != null)
                {
                    billeteraComprador.saldo_retenido -= pujaGanadora.monto;
                    billeteraComprador.saldo_total -= pujaGanadora.monto;
                    await _billeteraRepository.UpdateAsync(billeteraComprador);

                    await _ledgerRepository.AddAsyc(new Transaccion_Ledger
                    {
                        billetera_id = billeteraComprador.id,
                        subasta_id = subastaId,
                        tipo = "PAGO_SUBASTA",
                        monto = pujaGanadora.monto,
                        fecha = DateTime.Now
                    });
                }

                // B. Acreditar ingresos en la Billetera del Vendedor
                var billeteraVendedor = await _billeteraRepository.GetByUsuarioIdAsync(subasta.vendedor_id);
                if (billeteraVendedor != null)
                {
                    billeteraVendedor.saldo_disponible += pujaGanadora.monto;
                    billeteraVendedor.saldo_total += pujaGanadora.monto;
                    await _billeteraRepository.UpdateAsync(billeteraVendedor);

                    await _ledgerRepository.AddAsyc(new Transaccion_Ledger
                    {
                        billetera_id = billeteraVendedor.id,
                        subasta_id = subastaId,
                        tipo = "COBRO_SUBASTA",
                        monto = pujaGanadora.monto,
                        fecha = DateTime.Now
                    });
                }

                subasta.estado = EstadoSubasta.FINALIZADA;
            }
            else
            {
                subasta.estado = EstadoSubasta.DESIERTA;
            }

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}