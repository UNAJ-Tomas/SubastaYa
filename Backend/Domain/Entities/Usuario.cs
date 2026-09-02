using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class Usuario
    {
        public int id { get; set; }
        public string email { get; set; }
        public string nombre { get; set; }
        public string password_hash { get; set; }
        public DateTime fecha_registro { get; set; }
    }
}
