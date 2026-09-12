namespace Application.UseCases.Subasta.Queries
{
    public class ObtenerSubastasPorCompradorQuery
    {
        public int CompradorId { get; set; }

        public ObtenerSubastasPorCompradorQuery(int compradorId)
        {
            CompradorId = compradorId;
        }
    }
}