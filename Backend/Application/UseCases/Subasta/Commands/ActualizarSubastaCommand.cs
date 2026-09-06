namespace Application.UseCases.Subasta.Commands
{
    public record ActualizarSubastaCommand(
        int Id,
        string Titulo,
        string Descripcion,
        string UrlImagen,
        decimal PrecioBase,
        decimal IncrementoMinimo,
        DateTime FechaFin
    );
}