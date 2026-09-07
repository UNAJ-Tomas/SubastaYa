using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers
{
    public class RegistrarPujaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;

        public RegistrarPujaCommandHandler(
            ISubastaRepository subastaRepository, 
            IBilleteraRepository billeteraRepository)
        {
            _subastaRepository = subastaRepository;

        }

        public async Task<bool> Handle(RegistrarPujaCommand command, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.GetByIdAsync(command.SubastaId);

            if (subasta == null)
                throw new Exception("La subasta no existe.");

            if (subasta.estado != EstadoSubasta.ACTIVA || DateTime.Now > subasta.fecha_fin)
                throw new Exception("La subasta no se encuentra activa para recibir ofertas.");

            // Validar monto mínimo según las pujas existentes
            var pujaMaximaActual = subasta.Pujas != null && subasta.Pujas.Any()
                ? subasta.Pujas.Max(p => p.monto)
                : subasta.precio_base;

            var montoMinimoRequerido = subasta.Pujas != null && subasta.Pujas.Any()
                ? pujaMaximaActual + subasta.incremento_minimo
                : subasta.precio_base;

            if (command.Monto < montoMinimoRequerido)
                throw new Exception($"El monto ofertado debe ser de al menos {montoMinimoRequerido}.");

            
            var nuevaPuja = new Domain.Entities.Puja
            {
                subasta_id = command.SubastaId,
                comprador_id = command.CompradorId,
                monto = command.Monto,
                fecha_puja = DateTime.Now
            };

            subasta.Pujas ??= new List<Domain.Entities.Puja>();
            subasta.Pujas.Add(nuevaPuja);
            subasta.version = command.Version;

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}