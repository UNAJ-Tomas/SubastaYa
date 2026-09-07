namespace Application.UseCases.Billetera.Commands
{
    public record CargarSaldoCommand(
        int UsuarioId,
        decimal Monto
    );
}