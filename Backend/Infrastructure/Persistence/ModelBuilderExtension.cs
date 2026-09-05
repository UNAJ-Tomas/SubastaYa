using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                new Categoria { id = 1, nombre = "Tecnología", url_icono = null, },
                new Categoria { id = 2, nombre = "Coleccionables", url_icono = null, },
                new Categoria { id = 3, nombre = "Indumentaria", url_icono = null, },
                new Categoria { id = 4, nombre = "Vehículos", url_icono = null, }
            );
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { id = 1, nombre = "Creador de publicaciones", email = "vendedor@test.com", },
                new Usuario { id = 2, nombre = "Postor líder", email = "comprador1@test.com", },
                new Usuario { id = 3, nombre = "Postor habilitado", email = "comprador2@test.com", },
                new Usuario { id = 4, nombre = "Sin saldo", email = "sinfondos@test.com", }
            );
            modelBuilder.Entity<Billetera>().HasData(
                new Billetera { id = 1, usuario_id = 1, saldo_total = 0, saldo_retenido = 0, saldo_disponible = 0 },
                new Billetera { id = 2, usuario_id = 2, saldo_total = 150000, saldo_retenido = 45000, saldo_disponible = 105000 },
                new Billetera { id = 3, usuario_id = 3, saldo_total = 200000, saldo_retenido = 0, saldo_disponible = 200000 },
                new Billetera { id = 4, usuario_id = 4, saldo_total = 500, saldo_retenido = 0, saldo_disponible = 0 }
            );
            modelBuilder.Entity<Puja>().HasData(
                new Puja { id = 1, subasta_id = 1, comprador_id = 4, monto = 500, fecha_puja = DateTime.Now.AddMinutes(5), },
                new Puja { id = 3, subasta_id = 1, comprador_id = 2, monto = 45000, fecha_puja = DateTime.Now.AddMinutes(10) }
            );

            modelBuilder.Entity<Subasta>().HasData(
                new Subasta { id = 1, vendedor_id = 1, categoria_id = 1, titulo = "Celular", descripcion = "Celular Samsung", url_imagen = null, precio_base = 30000, incremento_minimo = 1000, fecha_inicio = DateTime.Now, fecha_fin = DateTime.Now.AddMinutes(30), estado = (EstadoSubasta)2, version = 0 },
                new Subasta { id = 2, vendedor_id = 1, categoria_id = 2, titulo = "Figura de acción", descripcion = "Figura de acción de colección", url_imagen = null, precio_base = 5000, incremento_minimo = 500, fecha_inicio = DateTime.Now, fecha_fin = DateTime.Now.AddMinutes(1), estado = (EstadoSubasta)2, version = 0 },
                new Subasta { id = 3, vendedor_id = 1, categoria_id = 3, titulo = "Camiseta", descripcion = "Camiseta de algodón", url_imagen = null, precio_base = 2000, incremento_minimo = 1000, fecha_inicio = DateTime.Now.AddHours(24), fecha_fin = DateTime.Now.AddMinutes(1), estado = (EstadoSubasta)1, version = 0 },
                new Subasta { id = 4, vendedor_id = 1, categoria_id = 4, titulo = "Bicicleta", descripcion = "Bicicleta de montaña", url_imagen = null, precio_base = 10000, incremento_minimo = 1000, fecha_inicio = DateTime.Now.AddDays(-7), fecha_fin = DateTime.Now.AddDays(-4), estado = (EstadoSubasta)2, version = 0 },
                new Subasta { id = 5, vendedor_id = 1, categoria_id = 1, titulo = "Tablet", descripcion = "Tablet de última generación", url_imagen = null, precio_base = 25000, incremento_minimo = 1500, fecha_inicio = DateTime.Now.AddDays(-7), fecha_fin = DateTime.Now.AddDays(-1), estado = (EstadoSubasta)2, version = 0 }
            );
            modelBuilder.Entity<Transaccion_Ledger>().HasData(
                new Transaccion_Ledger { id = 1, billetera_id = 2, tipo = "DEPOSITO", monto = 150000, fecha = DateTime.Now.AddDays(-10), subasta_id = 0 },
                new Transaccion_Ledger { id = 2, billetera_id = 3, tipo = "DEPOSITO", monto = 200000, fecha = DateTime.Now.AddDays(-10), subasta_id = 0 },
                new Transaccion_Ledger { id = 3, billetera_id = 4, tipo = "DEPOSITO", monto = 500, fecha = DateTime.Now.AddDays(-10), subasta_id = 0 },
                new Transaccion_Ledger { id = 4, billetera_id = 2, tipo = "RETENCION", monto = 45000, fecha = DateTime.Now.AddMinutes(10), subasta_id = 1 }
            );
        }
    }
}
