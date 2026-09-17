using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Subasta.Queries
{
    public class ObtenerSubastasActivasPorQuery
    {
        public int CompradorId { get; set; }
        public ObtenerSubastasActivasPorQuery(int id)
        {
            CompradorId = id;
        }
    }
}
