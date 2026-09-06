namespace Api.Controllers;

using Application.UseCases.Subasta.Commands;
using Application.UseCases.Subasta.Handlers;
using Application.UseCases.Subasta.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SubastasController : ControllerBase
{

    //atributos
    private readonly CrearSubastaCommandHandler _crearSubastaHandler;
    private readonly ObtenerSubastaPorIdQueryHandler _obtenerSubastaPorIdHandler;
    private readonly ObtenerSubastasActivasQueryHandler _obtenerSubastasActivasHandler;

    //constructor
    public SubastasController(
        CrearSubastaCommandHandler crearSubastaHandler,
        ObtenerSubastaPorIdQueryHandler obtenerSubastaPorIdHandler,
        ObtenerSubastasActivasQueryHandler obtenerSubastasActivasHandler)
    {
        _crearSubastaHandler = crearSubastaHandler;
        _obtenerSubastaPorIdHandler = obtenerSubastaPorIdHandler;
        _obtenerSubastasActivasHandler = obtenerSubastasActivasHandler;
    }


    //metodos

    [HttpPost]
    public async Task<IActionResult> Crear(CrearSubastaCommand command)
    {
        var idGenerado = await _crearSubastaHandler.HandleAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = idGenerado }, new { id = idGenerado, mensaje = "Subasta creada exitosamente" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new ObtenerSubastaPorIdQuery(id);
        var subasta = await _obtenerSubastaPorIdHandler.HandleAsync(query);

        if (subasta == null)
            return NotFound(new { mensaje = $"No se encontró la subasta con ID {id}" });

        return Ok(subasta);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActivas()
    {
        var subastas = await _obtenerSubastasActivasHandler.HandleAsync();
        return Ok(subastas);
    }
}