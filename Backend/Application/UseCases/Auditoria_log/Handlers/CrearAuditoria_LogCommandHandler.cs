using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Domain.Enums;

namespace Application.UseCases.Auditoria_log.Handlers
{
    public class CrearAuditoria_LogCommandHandler
    {
        private readonly IAuditoria_LogRepository _auditoria_logRepository;
        public CrearAuditoria_LogCommandHandler(IAuditoria_LogRepository auditoria_LogRepository)
        {
            _auditoria_logRepository = auditoria_LogRepository;
        }

        public async Task<int> HandleAsync(CrearAuditoria_LogCommand command, EstadoSubasta detalle)
        {
            var auditoriaLog = new Domain.Entities.Auditoria_Log
            {
                entidad = command.Entidad,
                entidad_id = command.Entidad_id,
                accion = command.Accion,
                usuario_id = command.Usuario_id,
                detalle_json = await CreateDetalle_json(command.Accion, detalle),
                fecha = DateTime.UtcNow,
            };

            await _auditoria_logRepository.AddAsync(auditoriaLog);

            return auditoriaLog.id;
        }
        private async Task<string> CreateDetalle_json(string accion, EstadoSubasta detalle)
        {
            switch(accion)
            {
                case "CIERRE_WORKER":
                    await Task.FromResult(System.Text.Json.JsonSerializer.Serialize("Activa a: " + detalle));
                    break;

                case "EXTENSION_TIEMPO":
                    await Task.FromResult(System.Text.Json.JsonSerializer.Serialize("Gatillada activada con tiempo restante: " + detalle));
                    break;

                case "PUJA_RECHAZADA":
                    await Task.FromResult(System.Text.Json.JsonSerializer.Serialize("Puja rechazada con monto: " + detalle));
                    break;

                case "ACREDITACION_MANUAL":
                    await Task.FromResult(System.Text.Json.JsonSerializer.Serialize("Acreditado un total de: " + detalle));
                    break;

                default:
                    await Task.FromResult(System.Text.Json.JsonSerializer.Serialize("Caso no contemplado"));
                    break;
            }
            return await Task.FromResult(System.Text.Json.JsonSerializer.Serialize(detalle));
        }
    }
}
