using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Subasta.Handlers;


public class CrearSubastaCommandHandler
{
    private readonly ISubastaRepository _subastaRepository;

    public CrearSubastaCommandHandler(ISubastaRepository subastaRepository)
    {
        _subastaRepository = subastaRepository;
    }

    public async Task<int> HandleAsync(CrearSubastaCommand command)
    {
        var subasta = new Domain.Entities.Subasta
        {
            vendedor_id = command.VendedorId,
            categoria_id = command.CategoriaId,
            titulo = command.Titulo,
            descripcion = command.Descripcion,
            url_imagen = command.UrlImagen,
            precio_base = command.PrecioBase,
            incremento_minimo = command.IncrementoMinimo,
            fecha_inicio = command.FechaInicio,
            fecha_fin = command.FechaFin,
            estado = EstadoSubasta.ACTIVA,
        };

        await _subastaRepository.AddAsync(subasta);

        return subasta.id;
    }
}