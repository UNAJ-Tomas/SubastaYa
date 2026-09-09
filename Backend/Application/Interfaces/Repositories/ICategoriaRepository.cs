using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoriaRepository
    {
        Task AddAsync(Categoria categoria);
        Task<Categoria?> GetByIdAsync(int id);
        Task<Categoria?> GetByNombreAsync(string nombre);
        Task<IEnumerable<Categoria>> GetAllAsync();
    }
}
