using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

    public class Usuario
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;

        // --- PROPIEDADES DE NAVEGACIÓN ---
        public Billetera? Billetera { get; set; }
        public ICollection<Subasta> Subastas { get; set; } = new List<Subasta>();
        public ICollection<Puja> Pujas { get; set; } = new List<Puja>();
    }

