using Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UseCases.Subasta.Handlers
{
    public class ObtenerSubastasActivasPorCompradorQueryHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasActivasPorCompradorQueryHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Subasta>> HandleAsync()
        {
            return await _subastaRepository.GetAllActivasAsync();
        }
    }
}