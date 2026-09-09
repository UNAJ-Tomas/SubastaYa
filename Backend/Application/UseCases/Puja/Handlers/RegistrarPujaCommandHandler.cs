using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class RegistrarPujaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public RegistrarPujaCommandHandler(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
        }

        public async Task<bool> Handle(RegistrarPujaCommand command, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.GetByIdAsync(command.SubastaId);

            if (subasta == null)
                throw new NotFoundException("La subasta no existe.");

            if (subasta.estado != EstadoSubasta.ACTIVA || DateTime.Now > subasta.fecha_fin)
                throw new ValidationException("La subasta no se encuentra activa para recibir ofertas.");

            // 1. Validar monto mínimo según las pujas existentes
            var pujaAnterior = subasta.Pujas?.OrderByDescending(p => p.monto).FirstOrDefault();

            var montoMinimoRequerido = pujaAnterior != null
                ? pujaAnterior.monto + subasta.incremento_minimo
                : subasta.precio_base;

            if (command.Monto < montoMinimoRequerido)
                throw new ValidationException($"El monto ofertado debe ser de al menos {montoMinimoRequerido}.");

            // 2. Validar billetera y saldo del nuevo comprador
            var billeteraNuevoComprador = await _billeteraRepository.GetByUsuarioIdAsync(command.CompradorId);
            if (billeteraNuevoComprador == null)
                throw new NotFoundException($"No se encontró la billetera para el usuario {command.CompradorId}.");

            if (billeteraNuevoComprador.saldo_disponible < command.Monto)
                throw new ValidationException("Saldo insuficiente en la billetera para realizar esta oferta.");

            // 3. Devolución de fondos al postor anterior (si existe)
            if (pujaAnterior != null)
            {
                var billeteraAnterior = await _billeteraRepository.GetByUsuarioIdAsync(pujaAnterior.comprador_id);
                if (billeteraAnterior != null)
                {
                    billeteraAnterior.saldo_disponible += pujaAnterior.monto;
                    billeteraAnterior.saldo_retenido -= pujaAnterior.monto;
                    await _billeteraRepository.UpdateAsync(billeteraAnterior);

                    await _ledgerRepository.AddAsyc(new Transaccion_Ledger
                    {
                        billetera_id = billeteraAnterior.id,
                        subasta_id = subasta.id,
                        tipo = "DEVOLUCION_PUJA",
                        monto = pujaAnterior.monto,
                        fecha = DateTime.Now
                    });
                }
            }

            // 4. Retención de fondos al nuevo comprador
            billeteraNuevoComprador.saldo_disponible -= command.Monto;
            billeteraNuevoComprador.saldo_retenido += command.Monto;
            await _billeteraRepository.UpdateAsync(billeteraNuevoComprador);

            await _ledgerRepository.AddAsyc(new Transaccion_Ledger
            {
                billetera_id = billeteraNuevoComprador.id,
                subasta_id = subasta.id,
                tipo = "RETENCION_PUJA",
                monto = command.Monto,
                fecha = DateTime.Now
            });

            // 5. Registrar la nueva puja y guardar cambios
            var nuevaPuja = new Domain.Entities.Puja
            {
                subasta_id = command.SubastaId,
                comprador_id = command.CompradorId,
                monto = command.Monto,
                fecha_puja = DateTime.Now
            };

            subasta.Pujas ??= new List<Domain.Entities.Puja>();
            subasta.Pujas.Add(nuevaPuja);

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}