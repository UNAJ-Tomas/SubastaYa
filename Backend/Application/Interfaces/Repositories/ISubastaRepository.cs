using Domain.Entities;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubastaRepository
    {
        Task AddAsync(Subasta subasta);
        Task<Subasta?> GetByIdAsync(int id);
        Task<IEnumerable<Subasta>> GetAllActivasAsync();

        Task<IEnumerable<Subasta>> GetAllWithPujasAsync();
        Task UpdateAsync(Subasta subasta);

        // Inicia una transacción de base de datos de forma sencilla
        Task<ITransaccion> IniciarTransaccionAsync(CancellationToken cancellationToken = default);
    }
}