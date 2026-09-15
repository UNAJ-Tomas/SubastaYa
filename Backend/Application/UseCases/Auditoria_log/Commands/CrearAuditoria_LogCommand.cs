using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auditoria_log.Commands
{
    public class CrearAuditoria_LogCommand
    {
        public string Entidad { get; set; } = string.Empty;
        public int Entidad_id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Detalle_json { get; set; } = string.Empty;
        public int? Usuario_id { get; set; }
    }
}
