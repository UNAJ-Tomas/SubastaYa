using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IBilleteraRepository
    {
        Task<Billetera?> GetByUsuarioIdAsync(int usuarioId);
        Task UpdateAsync(Billetera billetera);
    }
}