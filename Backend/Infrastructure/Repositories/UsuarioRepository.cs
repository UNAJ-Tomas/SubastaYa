using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly SubastaDbContext _context;
        public UsuarioRepository(SubastaDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Usuario usuario)
        {
            _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario.id;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(e=>e.email==email);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
             _context.Usuario.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
