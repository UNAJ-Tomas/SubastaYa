using Domain.Entities;
using Application.UseCases.Categoria.Handlers;
using Application.UseCases.Categoria.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ObtenerCategoriaPorIdQueryHandler _obtenerCategoriaPorIdHandler;
        private readonly ObtenerCategoriaPorNombreQueryHandler _obtenerCategoriaPorNombreHandler;
        private readonly ObtenerCategoriasQueryHandler _obtenerCategoriasQueryHandler;

        public CategoriaController(
        ObtenerCategoriaPorIdQueryHandler obtenerCategoriaPorIdHandler,
        ObtenerCategoriaPorNombreQueryHandler obtenerCategoriaPorNombreHandler,
        ObtenerCategoriasQueryHandler obtenerCategoriasQueryHandler)
        {
            _obtenerCategoriaPorIdHandler = obtenerCategoriaPorIdHandler;
            _obtenerCategoriaPorNombreHandler = obtenerCategoriaPorNombreHandler;
            _obtenerCategoriasQueryHandler = obtenerCategoriasQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _obtenerCategoriasQueryHandler.HandleAsync();
            return Ok(categorias);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetCategoriaPorId(int id)
        {
            var query = new ObtenerCategoriaPorIdQuery(id);
            var categoria = await _obtenerCategoriaPorIdHandler.HandleAsync(query);

            if (categoria == null)
                return NotFound(new { mensaje = $"No se encontró la categoría con ID {id}" });

            return Ok(categoria);
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<IActionResult> GetCategoriaPorNombre(string nombre)
        {
            var query = new ObtenerCategoriaPorNombreQuery(nombre);

            var categoria = await _obtenerCategoriaPorNombreHandler.HandleAsync(query);
            return Ok(categoria);
        }
    }
}
