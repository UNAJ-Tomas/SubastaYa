using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BilleteraRepository : IBilleteraRepository
    {
        private readonly SubastaDbContext _context;

        public BilleteraRepository(SubastaDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Billetera billetera)
        {
            await _context.Billetera.AddAsync(billetera);
            await _context.SaveChangesAsync(); 
        }

        public async Task<Billetera?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Billetera.FirstOrDefaultAsync(b => b.usuario_id == usuarioId);
        }

        public async Task UpdateAsync(Billetera billetera)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw new ConflictException("La billetera fue modificada, intente de nuevo.");
            }
        }
    }
}
