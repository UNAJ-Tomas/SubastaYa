using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Queries;

namespace Application.UseCases.Subasta.Handlers
{
    public class ObtenerSubastasPorVendedorQueryHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasPorVendedorQueryHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Subasta>> Handle(ObtenerSubastasPorVendedorQuery query)
        {
            var subastas = await _subastaRepository.GetAllActivasAsync();
            return subastas.Where(s => s.vendedor_id == query.VendedorId).ToList();
        }
    }
}