using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Queries;

namespace Application.UseCases.Subasta.Handlers
{
    public class ObtenerSubastasPorCompradorQueryHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasPorCompradorQueryHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Subasta>> Handle(ObtenerSubastasPorCompradorQuery query)
        {
            var subastas = await _subastaRepository.GetAllActivasAsync();
            return subastas.Where(s => s.Pujas != null && s.Pujas.Any(p => p.comprador_id == query.CompradorId)).ToList();
        }
    }
}