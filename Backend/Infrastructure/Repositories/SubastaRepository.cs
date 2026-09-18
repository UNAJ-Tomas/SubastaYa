namespace Infrastructure.Repositories;
using Application.Exceptions;

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

    public async Task<List<Subasta>> GetProgramadasIniciablesAsync(CancellationToken cancellationToken)
    {
        DateTime fechaActual = DateTime.UtcNow;
        return await _context.Subasta
        .Where(s =>
            s.estado == EstadoSubasta.PROGRAMADA &&
            s.fecha_inicio <= fechaActual &&
            s.fecha_fin >= fechaActual)
        .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subasta>> GetAllActivasAsync()
    {
        var fechaActual = DateTime.UtcNow;

        return await _context.Subasta
            .Include(s => s.Pujas) 
            .Where(s => s.estado == EstadoSubasta.ACTIVA && s.fecha_fin > fechaActual)
            .ToListAsync();
    }

    public async Task UpdateAsync(Subasta subasta)
    {
        try
        {
            _context.Subasta.Update(subasta);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("La subasta fue modificada por otro usuario.");
        }
    }

    public async Task<ITransaccion> IniciarTransaccionAsync(CancellationToken cancellationToken = default)
    {
        var efTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaccion(efTransaction);
    }
}