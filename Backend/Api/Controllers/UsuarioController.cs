using Application.UseCases.Usuario.Commands;
using Application.UseCases.Usuario.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly RegistrarUsuarioCommandHandler _registrarUsuarioCommandHandler;

        public UsuarioController(
            RegistrarUsuarioCommandHandler registrarUsuarioCommandHandler)
        {
            _registrarUsuarioCommandHandler = registrarUsuarioCommandHandler;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command)
        {
            var usuarioDto = await _registrarUsuarioCommandHandler.HandleAsync(command);
            return Ok(usuarioDto);
        }
    }
}
