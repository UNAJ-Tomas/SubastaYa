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
            // Buscamos directamente al usuario por su email usando el método de tu interfaz
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);

            // Verificamos si el usuario existe y si la contraseña coincide
            if (usuario == null || usuario.password_hash != loginDto.Password)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            // Si todo está bien, devolvemos los datos necesarios para el frontend
            return Ok(new { id = usuario.id, email = usuario.email, nombre = usuario.nombre });
        }

    }
}
