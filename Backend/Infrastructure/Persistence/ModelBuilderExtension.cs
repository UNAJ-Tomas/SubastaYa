using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { id = 1, nombre = "Tecnología", url_icono = "", },
                new Categoria { id = 2, nombre = "Coleccionables", url_icono = "", },
                new Categoria { id = 3, nombre = "Indumentaria", url_icono = "", },
                new Categoria { id = 4, nombre = "Vehículos", url_icono = "", }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { id = 1, nombre = "Creador de publicaciones", email = "vendedor@test.com", fecha_registro = DateTime.UtcNow.AddMonths(-1)},
                new Usuario { id = 2, nombre = "Postor líder", email = "comprador1@test.com", fecha_registro = DateTime.UtcNow.AddMonths(-1)},
                new Usuario { id = 3, nombre = "Postor habilitado", email = "comprador2@test.com", fecha_registro = DateTime.UtcNow.AddMonths(-1)},
                new Usuario { id = 4, nombre = "Sin saldo", email = "sinfondos@test.com", fecha_registro = DateTime.UtcNow.AddMonths(-1)}
            );
            modelBuilder.Entity<Billetera>().HasData(
                new Billetera { id = 1, usuario_id = 1, saldo_total = 100000, saldo_retenido = 0, saldo_disponible = 0 },
                new Billetera { id = 2, usuario_id = 2, saldo_total = 150000, saldo_retenido = 45000, saldo_disponible = 105000 },
                new Billetera { id = 3, usuario_id = 3, saldo_total = 200000, saldo_retenido = 0, saldo_disponible = 200000 },
                new Billetera { id = 4, usuario_id = 4, saldo_total = 500, saldo_retenido = 0, saldo_disponible = 0 }
            );
            modelBuilder.Entity<Puja>().HasData(
                new Puja { id = 1, subasta_id = 1, comprador_id = 4, monto = 500, fecha_puja = DateTime.UtcNow.AddMinutes(5), },
                new Puja { id = 3, subasta_id = 1, comprador_id = 2, monto = 45000, fecha_puja = DateTime.UtcNow.AddMinutes(10) }
            );

            modelBuilder.Entity<Subasta>().HasData(
                new Subasta { id = 1, vendedor_id = 1, categoria_id = 1, titulo = "Celular", descripcion = "Celular Samsung", url_imagen = "", precio_base = 30000, incremento_minimo = 1000, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddMinutes(30), estado = (EstadoSubasta)2 },
                new Subasta { id = 2, vendedor_id = 1, categoria_id = 2, titulo = "Figura de acción", descripcion = "Figura de acción de colección", url_imagen = "", precio_base = 5000, incremento_minimo = 500, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddMinutes(1), estado = (EstadoSubasta)2},
                new Subasta { id = 3, vendedor_id = 1, categoria_id = 3, titulo = "Camiseta", descripcion = "Camiseta de algodón", url_imagen = "", precio_base = 2000, incremento_minimo = 1000, fecha_inicio = DateTime.UtcNow.AddHours(24), fecha_fin = DateTime.UtcNow.AddMinutes(1), estado = (EstadoSubasta)1 },
                new Subasta { id = 4, vendedor_id = 1, categoria_id = 4, titulo = "Bicicleta", descripcion = "Bicicleta de montaña", url_imagen = "", precio_base = 10000, incremento_minimo = 1000, fecha_inicio = DateTime.UtcNow.AddDays(-7), fecha_fin = DateTime.UtcNow.AddDays(-4), estado = (EstadoSubasta)2 },
                new Subasta { id = 5, vendedor_id = 1, categoria_id = 1, titulo = "Tablet", descripcion = "Tablet de última generación", url_imagen = "", precio_base = 25000, incremento_minimo = 1500, fecha_inicio = DateTime.UtcNow.AddDays(-7), fecha_fin = DateTime.UtcNow.AddDays(-1), estado = (EstadoSubasta)2 }
            );
            modelBuilder.Entity<Transaccion_Ledger>().HasData(
                new Transaccion_Ledger { id = 1, billetera_id = 1, tipo = "DEPOSITO", monto = 150000, fecha = DateTime.UtcNow.AddDays(-10), subasta_id = null },
                new Transaccion_Ledger { id = 2, billetera_id = 2, tipo = "DEPOSITO", monto = 200000, fecha = DateTime.UtcNow.AddDays(-10), subasta_id = null },
                new Transaccion_Ledger { id = 3, billetera_id = 3, tipo = "DEPOSITO", monto = 500, fecha = DateTime.UtcNow.AddDays(-10), subasta_id = null },
                new Transaccion_Ledger { id = 4, billetera_id = 4, tipo = "RETENCION", monto = 45000, fecha = DateTime.UtcNow.AddMinutes(10), subasta_id = 1 }
            );
        }
        public static void SeedOpcional(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { id = 5, nombre = "Hogar", url_icono = "", }
            );
            modelBuilder.Entity<Subasta>().HasData(
                new Subasta { id = 6, vendedor_id = 1, categoria_id = 2, titulo = "Peluche", descripcion = "Un peluche de edición limitada.", url_imagen = "", precio_base = 100, incremento_minimo = 10, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddHours(5), estado = (EstadoSubasta)2 },
                new Subasta { id = 7, vendedor_id = 1, categoria_id = 3, titulo = "Remera", descripcion = "Una remera de alta calidad.", url_imagen = "", precio_base = 500, incremento_minimo = 50, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddHours(6), estado = (EstadoSubasta)2 },
                new Subasta { id = 8, vendedor_id = 1, categoria_id = 1, titulo = "Notebook", descripcion = "Una Notebook Dell.", url_imagen = "", precio_base = 25000, incremento_minimo = 2500, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddHours(12), estado = (EstadoSubasta)2 },
                new Subasta { id = 9, vendedor_id = 1, categoria_id = 4, titulo = "Auto Ford", descripcion = "Un auto de marca Ford.", url_imagen = "", precio_base = 2000, incremento_minimo = 1000, fecha_inicio = DateTime.UtcNow.AddHours(24), fecha_fin = DateTime.UtcNow.AddMinutes(1), estado = (EstadoSubasta)2 },
                new Subasta { id = 10, vendedor_id = 1, categoria_id = 5, titulo = "Casa", descripcion = "Una casa de lujo", url_imagen = "", precio_base = 500000, incremento_minimo = 5000, fecha_inicio = DateTime.UtcNow, fecha_fin = DateTime.UtcNow.AddHours(20), estado = (EstadoSubasta)2 }
            );
        }
    }
}
