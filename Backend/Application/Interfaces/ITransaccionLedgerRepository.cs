using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITransaccionLedgerRepository
    {
        Task<int> AddAsyc(Transaccion_Ledger transaccion);
        Task<IEnumerable<Transaccion_Ledger>> GetByBilleteraIdAsync(int billeteraId);
    }
}