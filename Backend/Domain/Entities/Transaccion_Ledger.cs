using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaccion_Ledger
    {
        public int id { get; set; }
        
        //FK
        public int billetera_id { get; set; }
        public int subasta_id { get; set; }

        public string tipo { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }

        // --- PROPIEDADES DE NAVEGACIÓN ---
        public Subasta? Subasta { get; set; }
        public Billetera? Billetera { get; set; }
    }
}
