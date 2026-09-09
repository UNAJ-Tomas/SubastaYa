using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.UseCases.Usuario.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Usuario.Handlers
{
    public class RegistrarUsuarioCommandHandler
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IBilleteraRepository _billeteraRepository;

        public RegistrarUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IBilleteraRepository billeteraRepository)
        {
            _usuarioRepository = usuarioRepository;
            _billeteraRepository = billeteraRepository;
        }

        //registramos al usuario
        public async Task<UsuarioDto> HandleAsync(RegistrarUsuarioCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
                throw new Exception("El email y la contraseña son obligatorios.");

            var existe = await _usuarioRepository.GetByEmailAsync(command.Email);
            if(existe!=null)
                throw new Exception("El email ya se encuentra registrado.");

            // 1. Crear Usuario
            var usuario = new Domain.Entities.Usuario
            {
                nombre = command.Nombre,
                email = command.Email,
                password_hash = command.Password // ( acá aplico hashing si corresponde)
            };

            var usuarioId = await _usuarioRepository.AddAsync(usuario);

            // 2. Crear automáticamente su Billetera asociada con saldo 0
            var billetera = new Domain.Entities.Billetera
            {
                usuario_id = usuarioId,
                saldo_disponible = 0,
                saldo_retenido = 0,
                saldo_total = 0
            };

            await _billeteraRepository.AddAsync(billetera);
            return new UsuarioDto(usuarioId, usuario.nombre, usuario.email);
        }
    }
}
