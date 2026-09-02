using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class Subasta
    {
        public int id { get; set; }
        public int vendedor_id { get; set; }
        public int categoria_id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string url_imagen { get; set; }
        public decimal precio_base { get; set; }
        public decimal incremento_minimo { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public string estado { get; set; }
        public int version { get; set; }
    }
}
