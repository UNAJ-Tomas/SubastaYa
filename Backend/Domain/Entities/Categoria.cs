namespace Domain.Entities
{
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string url_icono { get; set; } = null!;

        public ICollection<Subasta> Subastas { get; set; } = new List<Subasta>();
    }
}
