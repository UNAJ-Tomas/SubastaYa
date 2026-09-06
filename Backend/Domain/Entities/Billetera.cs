using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

    public class Billetera
    {
        public int id { get; set; }

        //FK
        public int usuario_id { get; set; }
        
        public decimal saldo_total { get; set; }
        public decimal saldo_retenido { get; set; }
        public decimal saldo_disponible { get; set; }
        public byte[] version { get; set; } = Array.Empty<byte>();



    // --- PROPIEDADES DE NAVEGACIÓN ---
    public Usuario Usuario { get; set; }
        public ICollection<Transaccion_Ledger> Transacciones { get; set; } = new List<Transaccion_Ledger>();
    }

