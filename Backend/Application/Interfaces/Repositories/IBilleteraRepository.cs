using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IBilleteraRepository
    {
        Task AddAsync(Billetera billetera);
        Task<Billetera?> GetByUsuarioIdAsync(int usuarioId);
        Task UpdateAsync(Billetera billetera);
    }
}