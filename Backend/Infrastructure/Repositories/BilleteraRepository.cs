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
        }

        public async Task<Billetera?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Billetera.FirstOrDefaultAsync(b => b.id == usuarioId);
        }

        public async Task UpdateAsync(Billetera billetera)
        {
            _context.Billetera.Update(billetera);
            await _context.SaveChangesAsync();
        }
    }
}
