using Application.Interfaces.Repositories;
using Application.UseCases.Categoria.Queries;
using Domain.Entities;


namespace Application.UseCases.Categoria.Handlers
{
    public class ObtenerCategoriaPorIdQueryHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoriaPorIdQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Domain.Entities.Categoria?> HandleAsync(ObtenerCategoriaPorIdQuery query)
        {
            return await _categoriaRepository.GetByIdAsync(query.Id);
        }
    }
}
