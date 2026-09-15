using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Auditoria_LogRepository : IAuditoria_LogRepository
    {
        private readonly SubastaDbContext _context;

        public Auditoria_LogRepository(SubastaDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Auditoria_Log auditoriaLog)
        {
            await _context.Auditoria_log.AddAsync(auditoriaLog);
            await _context.SaveChangesAsync();
        }
    }
}
