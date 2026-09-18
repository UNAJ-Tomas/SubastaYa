using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class Auditoria_logDbContext : DbContext
    {
        public Auditoria_logDbContext(DbContextOptions<Auditoria_logDbContext> options) : base(options) { }
    }
}
