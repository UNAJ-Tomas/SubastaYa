using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Application.UseCases.Auditoria_log.Handlers;
using Application.UseCases.Billetera.Commands;
using Domain.Entities;

namespace Application.UseCases.Billetera.Handlers
{
    public class CargarSaldoCommandHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;
        private readonly CrearAuditoria_LogCommandHandler _crearauditoriaCommandHandler;

        public CargarSaldoCommandHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository,
            CrearAuditoria_LogCommandHandler crearauditoriaCommandHandler)
        {
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
            _crearauditoriaCommandHandler = crearauditoriaCommandHandler;
        }

        public async Task<bool> HandleAsync(CargarSaldoCommand command, CancellationToken cancellationToken = default)
        {
            if (command.Monto <= 0)
                throw new Exception("El monto a cargar debe ser mayor a cero.");

            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(command.UsuarioId);

            if (billetera == null)
            {
                billetera = new Domain.Entities.Billetera
                {
                    usuario_id = command.UsuarioId,
                    saldo_disponible = command.Monto
                };

                await _billeteraRepository.AddAsync(billetera);
            }
            else
            {
                billetera.saldo_disponible += command.Monto;

                await _billeteraRepository.UpdateAsync(billetera);
            }

            CrearAuditoria_LogCommand auditoriaCommand = new CrearAuditoria_LogCommand
            {
                Entidad = "Billetera",
                Entidad_id = billetera.id,
                Accion = "ACREDITACION_MANUAL",
                Usuario_id = command.UsuarioId,
            };
            await _crearauditoriaCommandHandler.HandleAsync(auditoriaCommand, command.Monto.ToString());

            return true;
        }
    }
}