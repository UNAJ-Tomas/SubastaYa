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
    public async Task<IActionResult> ObtenerSaldo(int usuarioId)
    {
        var query = new ObtenerSaldoPorUsuarioQuery(usuarioId);
        var billeteraDto = await _obtenerSaldoPorUsuarioHandler.HandleAsync(query);

        return Ok(billeteraDto);
    }

    [HttpPost("cargar")]
    public async Task<IActionResult> CargarSaldo(CargarSaldoCommand command)
    {
        var resultado = await _cargarSaldoHandler.HandleAsync(command);
        return Ok(new { mensaje = "Saldo cargado exitosamente", exito = resultado });
    }

    [HttpGet("usuario/{usuarioId}/movimientos")]
    public async Task<IActionResult> ObtenerMovimientos(int usuarioId)
    {
        var resultado = await _obtenerHistorialQueryHandler.HandleAsync(usuarioId);
        return Ok(resultado);
    }
}