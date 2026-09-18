namespace Api.Controllers;

using Application.UseCases.Billetera.Commands;
using Application.UseCases.Billetera.Handlers;
using Application.UseCases.Billetera.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BilleterasController : ControllerBase
{
    private readonly CargarSaldoCommandHandler _cargarSaldoHandler;
    private readonly ObtenerSaldoPorUsuarioQueryHandler _obtenerSaldoPorUsuarioHandler;
    private readonly ObtenerHistorialTransaccionesQueryHandler _obtenerHistorialQueryHandler;

    public BilleterasController(
        CargarSaldoCommandHandler cargarSaldoHandler,
        ObtenerSaldoPorUsuarioQueryHandler obtenerSaldoPorUsuarioHandler,
        ObtenerHistorialTransaccionesQueryHandler obtenerHistorialQueryHandler)
    {
        _cargarSaldoHandler = cargarSaldoHandler;
        _obtenerSaldoPorUsuarioHandler = obtenerSaldoPorUsuarioHandler;
        _obtenerHistorialQueryHandler = obtenerHistorialQueryHandler;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> ObtenerSaldo(int usuarioId, CancellationToken cancellationToken)
    {
        var query = new ObtenerSaldoPorUsuarioQuery(usuarioId);
        var billeteraDto = await _obtenerSaldoPorUsuarioHandler.HandleAsync(query);
        if (billeteraDto == null)
            return NotFound(new { mensaje = $"No se encontró la billetera para el usuario {usuarioId}." });

        return Ok(billeteraDto);
    }

    [HttpPost("cargar")]
    public async Task<IActionResult> CargarSaldo(CargarSaldoCommand command, CancellationToken cancellationToken)
    {
        if (command == null || command.Monto <= 0)
            return BadRequest(new { mensaje = "El monto a cargar debe ser mayor a cero." });

        var resultado = await _cargarSaldoHandler.HandleAsync(command);

        
        return StatusCode(StatusCodes.Status201Created, new
        {
            mensaje = "Saldo cargado exitosamente",
            exito = resultado
        });
        /*
        return Ok(new { mensaje = "Saldo cargado exitosamente", exito = resultado });
        */
    }

    [HttpGet("usuario/{usuarioId}/movimientos")]
    public async Task<IActionResult> ObtenerMovimientos(int usuarioId, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerHistorialQueryHandler.HandleAsync(usuarioId);

        if (resultado == null || !resultado.Any())
            return NotFound(new { mensaje = "No se registraron movimientos para este usuario." });

        return Ok(resultado);
    }
}