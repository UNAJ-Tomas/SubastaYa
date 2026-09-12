using Application.DTOs;

namespace Application.UseCases.Subasta.Queries
{
    public class ObtenerSubastasConFiltroQuery
    {
        public FiltroSubastasDto Filtros { get; set; }

        public ObtenerSubastasConFiltroQuery(FiltroSubastasDto filtros)
        {
            Filtros = filtros;
        }
    }
}