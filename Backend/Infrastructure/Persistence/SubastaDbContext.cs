using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class SubastaDbContext:DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options):base(options){ }


        public DbSet<Auditoria_Log> Auditoria_log => Set<Auditoria_Log>();
        public DbSet<Billetera> Billetera => Set<Billetera>();
        public DbSet<Categoria> Categoria => Set<Categoria>();
        public DbSet<Puja> Puja => Set<Puja>();
        public DbSet<Subasta> Subasta => Set<Subasta>();
        public DbSet<Transaccion_Ledger> Transaccion_Ledgers => Set<Transaccion_Ledger>();
        public DbSet<Usuario> Usuario => Set<Usuario>();
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Billetera)
                .WithOne(b => b.Usuario)
                .HasForeignKey<Billetera>(b => b.usuario_id);

            modelBuilder.Entity<Billetera>()
                .Property(b => b.version)
                .IsRowVersion();

            modelBuilder.Entity<Subasta>()
                .Property(s => s.version)
                .IsRowVersion();

            modelBuilder.Entity<Billetera>(b =>
            {
                b.Property(p => p.saldo_total).HasPrecision(18, 2);
                b.Property(p => p.saldo_retenido).HasPrecision(18, 2);
                b.Property(p => p.saldo_disponible).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Subasta>(s =>
            {
                s.Property(p => p.precio_base).HasPrecision(18, 2);
                s.Property(p => p.incremento_minimo).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Transaccion_Ledger>(t =>
            {
                t.Property(p => p.monto).HasPrecision(18, 2);

                t.HasOne(x => x.Billetera)
                 .WithMany()
                 .HasForeignKey(x => x.billetera_id);

                t.HasOne(x => x.Subasta)
                 .WithMany()
                 .HasForeignKey(x => x.subasta_id)
                 .IsRequired(false);
            });

            modelBuilder.Entity<Subasta>().Property(s => s.estado).HasConversion<string>();
            modelBuilder.Entity<Transaccion_Ledger>().Property(t => t.tipo).HasConversion<string>();

            modelBuilder.Seed();
            modelBuilder.SeedOpcional();
        }

    }
}
