using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Commands;

namespace Application.UseCases.Billetera.Handlers
{
    public class CargarSaldoCommandHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public CargarSaldoCommandHandler(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<bool> HandleAsync(CargarSaldoCommand command, CancellationToken cancellationToken = default)
        {
            if (command.Monto <= 0)
                throw new Exception("El monto a cargar debe ser mayor a cero.");

            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(command.UsuarioId);

            if (billetera == null)
                throw new Exception("No se encontró la billetera del usuario.");

            billetera.saldo_disponible += command.Monto;

            await _billeteraRepository.UpdateAsync(billetera);
            return true;
        }
    }
}