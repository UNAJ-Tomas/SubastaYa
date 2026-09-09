using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Exceptions; 
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
                throw new NotFoundException($"No se encontró la billetera para el usuario con ID {query.UsuarioId}.");

            return new BilleteraDto(
                billetera.id,
                billetera.usuario_id,
                billetera.saldo_disponible,
                billetera.saldo_retenido,
                billetera.saldo_total
            );
        }
    }
}