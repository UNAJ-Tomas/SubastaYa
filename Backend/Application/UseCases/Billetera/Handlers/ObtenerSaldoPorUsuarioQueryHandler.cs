using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Queries;

namespace Application.UseCases.Billetera.Handlers
{
    public class ObtenerSaldoPorUsuarioQueryHandler
    {
        private readonly IBilleteraRepository _billeteraRepository;

        public ObtenerSaldoPorUsuarioQueryHandler(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<BilleteraDto> HandleAsync(ObtenerSaldoPorUsuarioQuery query, CancellationToken cancellationToken = default)
        {
            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(query.UsuarioId);

            if (billetera == null)
                throw new Exception("No se encontró la billetera del usuario.");

            return new BilleteraDto(
                billetera.usuario_id,
                billetera.saldo_disponible,
                billetera.saldo_retenido,
                billetera.saldo_total
            );
        }
    }
}