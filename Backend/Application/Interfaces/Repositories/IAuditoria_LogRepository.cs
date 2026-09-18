using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IAuditoria_LogRepository
    {
        Task AddAsync(Auditoria_Log auditoriaLog);
    }
}
