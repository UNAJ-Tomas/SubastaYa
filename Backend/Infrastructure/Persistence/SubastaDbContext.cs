using Application.Interfaces;
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

        DbSet<Auditoria_Log> Auditoria_log => Set<Auditoria_Log>();
        DbSet<Billetera> Billetera => Set<Billetera>();
        DbSet<Categoria> Categoria => Set<Categoria>();
        DbSet<Puja> Puja => Set<Puja>();
        DbSet<Subasta> Subasta => Set<Subasta>();
        DbSet<Transaccion_Ledger> Transaccion_Ledgers => Set<Transaccion_Ledger>();
        DbSet<Usuario> Usuario => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Relación 1:1 obligatoria (EF necesita saber qué tabla lleva la FK)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Billetera)
                .WithOne(b => b.Usuario)
                .HasForeignKey<Billetera>(b => b.usuario_id);

            // 2. Subasta (Evita eliminaciones en cascada ambiguas + Concurrencia + Decimales)
            modelBuilder.Entity<Subasta>(entity =>
            {
                entity.HasOne(s => s.Vendedor)
                    .WithMany()
                    .HasForeignKey(s => s.vendedor_id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(s => s.version).IsRowVersion();
                entity.Property(s => s.precio_base).HasPrecision(18, 2);
                entity.Property(s => s.incremento_minimo).HasPrecision(18, 2);
            });

            // 3. Puja (Evita borrado en cascada del comprador + Decimal)
            modelBuilder.Entity<Puja>(entity =>
            {
                entity.HasOne(p => p.Comprador)
                    .WithMany()
                    .HasForeignKey(p => p.comprador_id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.monto).HasPrecision(18, 2);
            });

            // 4. Transaccion_Ledger (Subasta es opcional según diagrama + Decimal)
            modelBuilder.Entity<Transaccion_Ledger>(entity =>
            {
                entity.HasOne(t => t.Subasta)
                    .WithMany()
                    .HasForeignKey(t => t.subasta_id)
                    .IsRequired(false);

                entity.Property(t => t.monto).HasPrecision(18, 2);
            });

            // 5. Auditoria_Log (Usuario es opcional cuando la acción la ejecuta el Worker)
            modelBuilder.Entity<Auditoria_Log>(entity =>
            {
                entity.HasOne(a => a.Usuario)
                    .WithMany()
                    .HasForeignKey(a => a.usuario_id)
                    .IsRequired(false);
            });

            // 6. Billetera (Concurrencia + Decimales)
            modelBuilder.Entity<Billetera>(entity =>
            {
                entity.Property(b => b.version).IsRowVersion();
                entity.Property(b => b.saldo_total).HasPrecision(18, 2);
                entity.Property(b => b.saldo_retenido).HasPrecision(18, 2);
                entity.Property(b => b.saldo_disponible).HasPrecision(18, 2);
            });





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
