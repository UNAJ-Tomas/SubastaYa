using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Usuario.Commands;
using Application.UseCases.Usuario.Handlers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuarioCommandHandler _registrarUsuarioCommandHandler;
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuariosController(
            RegistrarUsuarioCommandHandler registrarUsuarioCommandHandler,
            IUsuarioRepository usuarioRepository)
        {
            _registrarUsuarioCommandHandler = registrarUsuarioCommandHandler;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
        {
            var usuarioDto = await _registrarUsuarioCommandHandler.HandleAsync(command,cancellationToken);
            return Ok(usuarioDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);

            if (usuario == null || usuario.password_hash != loginDto.Password)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            return Ok(new { id = usuario.id, email = usuario.email, nombre = usuario.nombre });
        }

    }
}
