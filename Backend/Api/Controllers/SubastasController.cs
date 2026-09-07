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
    private readonly ActualizarSubastaCommandHandler _actualizarSubastaHandler;
    private readonly CancelarSubastaCommandHandler _cancelarSubastaHandler;
    private readonly ObtenerSubastaPorIdQueryHandler _obtenerSubastaPorIdHandler;
    private readonly ObtenerSubastasActivasQueryHandler _obtenerSubastasActivasHandler;
    private readonly RegistrarPujaCommandHandler _registrarPujaCommandHandler;

    //constructor
    public SubastasController(
    RegistrarPujaCommandHandler registrarPujaCommandHandler,
        CrearSubastaCommandHandler crearSubastaHandler,
        ActualizarSubastaCommandHandler actualizarSubastaHandler,
        CancelarSubastaCommandHandler cancelarSubastaHandler,
        ObtenerSubastaPorIdQueryHandler obtenerSubastaPorIdHandler,
        ObtenerSubastasActivasQueryHandler obtenerSubastasActivasHandler)
    {
        _crearSubastaHandler = crearSubastaHandler;
        _actualizarSubastaHandler = actualizarSubastaHandler;
        _cancelarSubastaHandler = cancelarSubastaHandler;
        _obtenerSubastaPorIdHandler = obtenerSubastaPorIdHandler;
        _obtenerSubastasActivasHandler = obtenerSubastasActivasHandler;
        _registrarPujaCommandHandler = registrarPujaCommandHandler;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarSubastaCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la ruta no coincide con el cuerpo de la solicitud.");

        var resultado = await _actualizarSubastaHandler.Handle(command);
        return Ok(new { mensaje = "Subasta actualizada correctamente", exito = resultado });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var command = new CancelarSubastaCommand(id);
        var resultado = await _cancelarSubastaHandler.Handle(command);
        return Ok(new { mensaje = "Subasta cancelada correctamente", exito = resultado });
    }

    [HttpPost("{id}/pujas")]
    public async Task<IActionResult> RegistrarPuja(int id, RegistrarPujaCommand command)
    {
        if (id != command.SubastaId)
            return BadRequest("El ID de la ruta no coincide con el cuerpo de la solicitud.");

        var resultado = await _registrarPujaCommandHandler.Handle(command);
        return Ok(new { mensaje = "Puja registrada exitosamente", exito = resultado });
    }
}