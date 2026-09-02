using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Puja
    {
        public int id { get; set; }
        public int subasta_id { get; set; }
        public int comprador_id { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha_puja { get; set; }
    }
}
