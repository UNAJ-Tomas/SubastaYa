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
        Task<IEnumerable<Domain.Entities.Subasta>> GetAllAsync(int compradorId);

        Task<ITransaccion> IniciarTransaccionAsync(CancellationToken cancellationToken = default);
    }
}