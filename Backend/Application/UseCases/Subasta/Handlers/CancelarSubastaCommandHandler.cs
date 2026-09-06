using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class CancelarSubastaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public CancelarSubastaCommandHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<bool> Handle(CancelarSubastaCommand command, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.GetByIdAsync(command.Id);

            if (subasta == null)
                throw new Exception("La subasta no existe.");

            if (subasta.Pujas != null && subasta.Pujas.Any())
                throw new Exception("No se puede cancelar una subasta que ya recibió pujas.");

            subasta.estado = EstadoSubasta.DESIERTA;

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}