using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransaccionLedgerRepository : ITransaccionLedgerRepository
    {
        private readonly SubastaDbContext _context;

        public TransaccionLedgerRepository(SubastaDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsyc(Transaccion_Ledger transaccion)
        {
            try
            {
                await _context.Set<Transaccion_Ledger>().AddAsync(transaccion);
                await _context.SaveChangesAsync();
                return transaccion.id;
            }
            catch(DbUpdateConcurrencyException)
            {
                throw new ConflictException("La operación fue modificada por otro usuario.");
            }
        }

        public async Task<IEnumerable<Transaccion_Ledger>> GetByBilleteraIdAsync(int billeteraId)
        {
            return await _context.Set<Transaccion_Ledger>()
                .Where(t => t.billetera_id == billeteraId)
                .OrderByDescending(t => t.fecha)
                .ToListAsync();
        }
    }
}