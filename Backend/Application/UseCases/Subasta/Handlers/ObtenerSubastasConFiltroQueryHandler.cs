using Application.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class ObtenerSubastasConFiltroQueryHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasConFiltroQueryHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Subasta>> Handle(FiltroSubastasDto filtro)
        {
            var subastas = await _subastaRepository.GetAllActivasAsync();
            var query = subastas.AsQueryable();

            if (filtro.Estado.HasValue)
                query = query.Where(s => s.estado == filtro.Estado.Value);

            if (filtro.CategoriaId.HasValue)
                query = query.Where(s => s.categoria_id == filtro.CategoriaId.Value);

            if (filtro.PrecioMin.HasValue)
                query = query.Where(s => (s.Pujas.Any() ? s.Pujas.Max(p => p.monto) : s.precio_base) >= filtro.PrecioMin.Value);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(s => (s.Pujas.Any() ? s.Pujas.Max(p => p.monto) : s.precio_base) <= filtro.PrecioMax.Value);

            query = filtro.Orden switch
            {
                "tiempo_asc" => query.OrderBy(s => s.fecha_fin),
                "precio_asc" => query.OrderBy(s => s.Pujas.Any() ? s.Pujas.Max(p => p.monto) : s.precio_base),
                "precio_desc" => query.OrderByDescending(s => s.Pujas.Any() ? s.Pujas.Max(p => p.monto) : s.precio_base),
                _ => query.OrderBy(s => s.fecha_fin)
            };

            return query.ToList();
        }
    }
}