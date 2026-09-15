namespace Domain.Entities;
public class Auditoria_Log
{
        public int id { get; set; }
        public string entidad { get; set; }
        public int entidad_id { get; set; }
        public int? usuario_id { get; set; }
        public string accion { get; set; }
        public string detalle_json { get; set; }
        public DateTime fecha { get; set; }


        // --- PROPIEDADES DE NAVEGACIÓN ---
        public Usuario? Usuario { get; set; }
}
