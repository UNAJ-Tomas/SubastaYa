using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class SubastaDbContext:DbContext
    {
        public SubastaDbContext(DbContextOptions<SubastaDbContext> options):base(options){ }

        //tablas

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

            // 1. Relación 1:1 Usuario - Billetera (EF necesita saber qué tabla lleva la FK)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Billetera)
                .WithOne(b => b.Usuario)
                .HasForeignKey<Billetera>(b => b.usuario_id);

            // 2. Concurrencia optimista (Optimistic Locking) en Billetera
            modelBuilder.Entity<Billetera>()
                .Property(b => b.version)
                .IsRowVersion();

            // 3. Concurrencia optimista (Optimistic Locking) en Subasta
            modelBuilder.Entity<Subasta>()
                .Property(s => s.version)
                .IsRowVersion();

            // 4. Precisión decimal para dinero en Billetera
            modelBuilder.Entity<Billetera>(b =>
            {
                b.Property(p => p.saldo_total).HasPrecision(18, 2);
                b.Property(p => p.saldo_retenido).HasPrecision(18, 2);
                b.Property(p => p.saldo_disponible).HasPrecision(18, 2);
            });

            // 5. Precisión decimal para dinero en Subasta
            modelBuilder.Entity<Subasta>(s =>
            {
                s.Property(p => p.precio_base).HasPrecision(18, 2);
                s.Property(p => p.incremento_minimo).HasPrecision(18, 2);
            });


            // Conversión de Enums a String
            modelBuilder.Entity<Subasta>().Property(s => s.estado).HasConversion<string>();
            modelBuilder.Entity<Transaccion_Ledger>().Property(t => t.tipo).HasConversion<string>();

            modelBuilder.Seed();
        }

    }
}
