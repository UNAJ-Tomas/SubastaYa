using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.UseCases.Usuario.Commands;
using Application.UseCases.Usuario.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly RegistrarUsuarioCommandHandler _registrarUsuarioCommandHandler;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        // Constructor que inyecta todas las dependencias necesarias
        public UsuariosController(
            RegistrarUsuarioCommandHandler registrarUsuarioCommandHandler,
            IUsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            _registrarUsuarioCommandHandler = registrarUsuarioCommandHandler;
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
        {
            var usuarioDto = await _registrarUsuarioCommandHandler.HandleAsync(command, cancellationToken);
            return Ok(usuarioDto);
        }

        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email))
            {
                return BadRequest("No se recibieron los datos de acceso.");
            }

            var emailLimpio = loginDto.Email.Trim();
            var usuario = await _usuarioRepository.GetByEmailAsync(emailLimpio);

            if (usuario == null)
            {
                return Unauthorized($"El correo '{emailLimpio}' no está registrado.");
            }

            var passwordLimpia = loginDto.Password?.Trim() ?? string.Empty;
            if (usuario.password_hash != passwordLimpia)
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Generación del Token JWT
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "120");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                id = usuario.id,
                email = usuario.email,
                nombre = usuario.nombre
            });
        }
    }
}