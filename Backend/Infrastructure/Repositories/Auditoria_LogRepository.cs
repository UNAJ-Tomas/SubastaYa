using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Auditoria_LogRepository : IAuditoria_LogRepository
    {
        private readonly IDbContextFactory<SubastaDbContext> _contextFactory;
        public Auditoria_LogRepository(IDbContextFactory<SubastaDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task AddAsync(Auditoria_Log auditoriaLog)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            await context.Auditoria_log.AddAsync(auditoriaLog);

            await context.SaveChangesAsync();
        }
    }
}
