using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Application.UseCases.Auditoria_log.Handlers;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class FinalizarSubastaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;
        private readonly CrearAuditoria_LogCommandHandler _crearauditoriaCommandHandler;

        public FinalizarSubastaCommandHandler(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository,
            CrearAuditoria_LogCommandHandler crearauditoriaCommandHandler)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
            _crearauditoriaCommandHandler = crearauditoriaCommandHandler;
        }
        public async Task<bool> HandleAsync(int subastaId)
        {
            var subasta = await _subastaRepository.GetByIdAsync(subastaId);
            if (subasta == null)
                throw new NotFoundException($"La subasta con ID {subastaId} no existe.");

            if (subasta.estado != EstadoSubasta.ACTIVA)
                throw new ValidationException("Solo se pueden finalizar subastas que se encuentren activas.");

            var pujaGanadora = subasta.Pujas?.OrderByDescending(p => p.monto).FirstOrDefault();

            if (pujaGanadora != null)
            {
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
            CrearAuditoria_LogCommand command = new CrearAuditoria_LogCommand
            {
                Entidad = "Subasta",
                Entidad_id = subastaId,
                Accion = "CIERRE_WORKER",
                Usuario_id = null,
            };
            await _crearauditoriaCommandHandler.HandleAsync(command, subasta.estado.ToString());

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}