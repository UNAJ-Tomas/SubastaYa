using Application.Interfaces.Repositories;
using Application.UseCases.Categoria.Queries;
using Domain.Entities;

namespace Application.UseCases.Categoria.Handlers
{
    public class ObtenerCategoriasQueryHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Categoria?>> HandleAsync()
        {
            return await _categoriaRepository.GetAllAsync();
        }
    }
}
