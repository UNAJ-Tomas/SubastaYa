namespace Infrastructure.Repositories;

using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class SubastaRepository : ISubastaRepository
{
    private readonly SubastaDbContext _context;

    public SubastaRepository(SubastaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Subasta subasta)
    {
        await _context.Subasta.AddAsync(subasta);
        await _context.SaveChangesAsync();
    }

    public async Task<Subasta?> GetByIdAsync(int id)
    {
        return await _context.Subasta
            .Include(s => s.Pujas)
            .FirstOrDefaultAsync(s => s.id == id);
    }

    public async Task<IEnumerable<Subasta>> GetAllActivasAsync()
    {
        return await _context.Subasta
            .Where(s => s.estado==EstadoSubasta.ACTIVA)
            .ToListAsync();
    }

    public async Task UpdateAsync(Subasta subasta)
    {
        _context.Subasta.Update(subasta);
        await _context.SaveChangesAsync();
    }
}