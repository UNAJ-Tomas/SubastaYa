using System.Text.Json;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Domain.Entities;
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

        public async Task<int> HandleAsync(CrearAuditoria_LogCommand command, string detalle)
        {
            var auditoriaLog = new Domain.Entities.Auditoria_Log
            {
                entidad = command.Entidad,
                entidad_id = command.Entidad_id,
                accion = command.Accion,
                usuario_id = command.Usuario_id,
                detalle_json = CreateDetalle_json(command.Accion, detalle),
                fecha = DateTime.UtcNow,
            };

            await _auditoria_logRepository.AddAsync(auditoriaLog);

            return auditoriaLog.id;
        }
        private string CreateDetalle_json(string accion, string detalle)
        {
            string json;
            object detalle_json;
            switch(accion)
            {
                case "CIERRE_WORKER":
                    detalle_json = new
                    {
                        estado_anterior = "ACTIVA",
                        estado_nuevo = detalle,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "EXTENSION_TIEMPO":
                    detalle_json = new
                    {
                       tiempo_anterior = detalle + " segundos.",
                       tiempo_nuevo = "60 segundos",
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "PUJA_RECHAZADA":
                    detalle_json = new
                    {
                        motivo = detalle,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "ACREDITACION_MANUAL":
                    detalle_json = new
                    {
                        monto_acreditado = detalle,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "ABRIR_WORKER":
                    detalle_json = new
                    {
                        estado_anterior = "PROGRAMADA",
                        estado_nuevo = detalle,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "PUJA_INVALIDA":
                    detalle_json = new
                    {
                        motivo = detalle,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                case "ERROR_AUDITORIA":
                    detalle_json = new
                    {
                        error = detalle,
                        Accion = accion,
                    };
                    json = JsonSerializer.Serialize(detalle_json);
                    break;

                default:
                    detalle_json = new
                    {
                        estado = detalle,
                    };
                    json = json = JsonSerializer.Serialize(detalle_json);
                    break;
            }
            return json;
        }
    }
}
