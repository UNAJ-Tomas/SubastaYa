namespace Application.UseCases.Subasta.Commands
{
    public record RegistrarPujaCommand(
        int SubastaId,
        int CompradorId,
        decimal Monto,
        byte[] Version
    );
}