namespace Application.UseCases.Subasta.Queries
{
    public class ObtenerSubastasPorVendedorQuery
    {
        public int VendedorId { get; set; }

        public ObtenerSubastasPorVendedorQuery(int vendedorId)
        {
            VendedorId = vendedorId;
        }
    }
}