using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Commands;

namespace Application.UseCases.Billetera.Handlers
{
    public class CargarSaldoCommandHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public CargarSaldoCommandHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository)
        {
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
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

            return true;
        }
    }
}