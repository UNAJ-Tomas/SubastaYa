namespace Application.DTOs
{
    public record BilleteraDto(
        int UsuarioId,
        decimal SaldoDisponible,
        decimal SaldoRetenido,
        decimal SaldoTotal
    );
}