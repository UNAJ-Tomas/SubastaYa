using Domain.Entities;
using System;
using System.Collections.Generic;
using Domain.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Subasta
{
    public int id { get; set; }

    //FK
    public int vendedor_id { get; set; }
    public int categoria_id { get; set; }
    
    
    public string titulo { get; set; }
    public string descripcion { get; set; }
    public string url_imagen { get; set; }
    public decimal precio_base { get; set; }
    public decimal incremento_minimo { get; set; }
    public DateTime fecha_inicio { get; set; }
    public DateTime fecha_fin { get; set; }
    public EstadoSubasta estado { get; set; } = EstadoSubasta.ACTIVA;
    public int version { get; set; }


    // --- PROPIEDADES DE NAVEGACIÓN ---
    public Usuario? Vendedor { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Puja> Pujas { get; set; } = new List<Puja>();
}
