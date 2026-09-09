using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly SubastaDbContext _context;

        public CategoriaRepository(SubastaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _context.Categoria.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categoria
            .FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Categoria?> GetByNombreAsync(string nombre)
        {
            return await _context.Categoria
            .FirstOrDefaultAsync(c => c.nombre.ToLower() == nombre.ToLower());
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Categoria.ToListAsync();
        }
    }
}
