using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;

namespace Application.UseCases.Subasta.Handlers
{
    public class ActualizarSubastaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;

        public ActualizarSubastaCommandHandler(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<bool> Handle(ActualizarSubastaCommand command, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.GetByIdAsync(command.Id);

            if (subasta == null)
                throw new Exception("La subasta no existe.");

            if (subasta.Pujas != null && subasta.Pujas.Any())
                throw new Exception("No se puede editar una subasta que ya tiene ofertas realizadas.");

            subasta.titulo = command.Titulo;
            subasta.descripcion = command.Descripcion;
            subasta.url_imagen = command.UrlImagen;
            subasta.precio_base = command.PrecioBase;
            subasta.incremento_minimo = command.IncrementoMinimo;
            subasta.fecha_fin = command.FechaFin;

            await _subastaRepository.UpdateAsync(subasta);
            return true;
        }
    }
}