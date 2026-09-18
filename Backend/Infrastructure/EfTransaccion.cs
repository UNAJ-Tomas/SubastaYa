using Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence
{
    public class EfTransaccion : ITransaccion
    {
        private readonly IDbContextTransaction _transaction;

        public EfTransaccion(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
           => _transaction.CommitAsync(cancellationToken);

        public Task RollbackAsync(CancellationToken cancellationToken = default)
            => _transaction.RollbackAsync(cancellationToken);

        public void Dispose()
            => _transaction.Dispose();
    }
}