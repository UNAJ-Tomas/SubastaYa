using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.UseCases.Billetera.Handlers
{
    public class ObtenerHistorialTransaccionesQueryHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;

        public ObtenerHistorialTransaccionesQueryHandler(
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository)
        {
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
        }

        public async Task<IEnumerable<Transaccion_Ledger>> HandleAsync(int usuarioId)
        {
            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(usuarioId);
            if (billetera == null)
                throw new NotFoundException($"No se encontró una billetera asociada al usuario {usuarioId}.");

            return await _ledgerRepository.GetByBilleteraIdAsync(billetera.id);
        }
    }
}