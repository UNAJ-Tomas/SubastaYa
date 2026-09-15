using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<int> AddAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task UpdateAsync(Usuario usuario);
    }
}
