using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISubastaRepository
    {
        Task AddAsync(Subasta subasta);
        Task<Subasta?> GetByIdAsync(int id);
        Task<List<Subasta>> GetProgramadasIniciablesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Subasta>> GetAllActivasAsync();
        Task UpdateAsync(Subasta subasta);

        // Inicia una transacción de base de datos de forma sencilla
        Task<ITransaccion> IniciarTransaccionAsync(CancellationToken cancellationToken = default);
    }
}