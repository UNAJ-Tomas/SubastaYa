using Domain.Enums;

namespace Application.DTOs
{
    public class FiltroSubastasDto
    {
        public EstadoSubasta? Estado { get; set; }
        public int? CategoriaId { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? Orden { get; set; } // "tiempo_asc", "precio_asc", "precio_desc"
    }
}
