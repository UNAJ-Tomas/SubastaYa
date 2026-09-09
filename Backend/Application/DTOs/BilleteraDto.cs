namespace Application.DTOs
{
    public record BilleteraDto(
        int id,
        int UsuarioId,
        decimal SaldoDisponible,
        decimal SaldoRetenido,
        decimal SaldoTotal
    );
}