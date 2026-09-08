namespace Application.UseCases.Categoria.Queries
{
    public class ObtenerCategoriaPorIdQuery
    {
        public int Id { get; set; }

        public ObtenerCategoriaPorIdQuery(int id)
        {
            Id = id;
        }
    }
}
