namespace Domain.Entities
{
    public class Puja
    {
        public int id { get; set; }

        //FK
        public int subasta_id { get; set; }
        public int comprador_id { get; set; }
        
        public decimal monto { get; set; }
        public DateTime fecha_puja { get; set; }


        // --- PROPIEDADES DE NAVEGACIÓN ---
        public Subasta? Subasta { get; set; }
        public Usuario? Comprador { get; set; }

    }
}
