namespace Application.UseCases.Subasta.Queries;

public class ObtenerSubastaPorIdQuery
{
    public int Id { get; set; }

    public ObtenerSubastaPorIdQuery(int id)
    {
        Id = id;
    }
}