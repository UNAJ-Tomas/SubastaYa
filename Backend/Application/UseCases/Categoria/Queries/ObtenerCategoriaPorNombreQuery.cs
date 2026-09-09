namespace Application.UseCases.Categoria.Queries
{
    public class ObtenerCategoriaPorNombreQuery
    {
        public string Nombre { get; set; }

        public ObtenerCategoriaPorNombreQuery(string nombre)
        {
            Nombre = nombre;
        }
    }
}
