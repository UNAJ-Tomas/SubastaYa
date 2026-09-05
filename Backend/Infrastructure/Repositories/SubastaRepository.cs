using static System.Net.Mime.MediaTypeNames;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories;



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
        return await _context.Subasta.FirstOrDefaultAsync(s => s.id == id);
    }

    public async Task<IEnumerable<Subasta>> GetAllActivasAsync()
    {
        return await _context.Subasta.ToListAsync();
    }
}