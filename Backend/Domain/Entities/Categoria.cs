using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string url_icono { get; set; } = null!;

        // --- PROPIEDADES DE NAVEGACIÓN ---
        public ICollection<Subasta> Subastas { get; set; } = new List<Subasta>();
    }
}
