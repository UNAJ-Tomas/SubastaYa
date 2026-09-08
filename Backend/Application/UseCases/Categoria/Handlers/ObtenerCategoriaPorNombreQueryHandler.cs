using Application.Interfaces.Repositories;
using Application.UseCases.Categoria.Queries;
using Domain.Entities;

namespace Application.UseCases.Categoria.Handlers
{
    public class ObtenerCategoriaPorNombreQueryHandler
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public ObtenerCategoriaPorNombreQueryHandler(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Domain.Entities.Categoria?> HandleAsync(ObtenerCategoriaPorNombreQuery query)
        {
            return await _categoriaRepository.GetByNombreAsync(query.Nombre);
        }
    }
}
