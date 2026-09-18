using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Application.UseCases.Auditoria_log.Handlers;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class IniciarSubastasProgramadasCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly CrearAuditoria_LogCommandHandler _crearauditoriaCommandHandler;

        public IniciarSubastasProgramadasCommandHandler(
            ISubastaRepository subastaRepository,
            CrearAuditoria_LogCommandHandler crearauditoriaCommandHandler)
        {
            _subastaRepository = subastaRepository;
            _crearauditoriaCommandHandler = crearauditoriaCommandHandler;
        }

        public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            var subastas = await _subastaRepository.GetProgramadasIniciablesAsync(cancellationToken);
            foreach (var subasta in subastas)
            {
                cancellationToken.ThrowIfCancellationRequested();

                subasta.estado = EstadoSubasta.ACTIVA;
                CrearAuditoria_LogCommand command = new CrearAuditoria_LogCommand
                {
                    Entidad = "Subasta",
                    Entidad_id = subasta.id,
                    Accion = "ABRIR_WORKER",
                    Usuario_id = null,
                };
                await _subastaRepository.UpdateAsync(subasta);
                await _crearauditoriaCommandHandler.HandleAsync(command, subasta.estado.ToString());
            }
            return true;
        }
    }
}
