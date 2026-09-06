namespace Application.UseCases.Subasta.Handlers;

using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Subasta.Queries;
using Domain.Entities;

public class ObtenerSubastaPorIdQueryHandler
{
    private readonly ISubastaRepository _subastaRepository;

    public ObtenerSubastaPorIdQueryHandler(ISubastaRepository subastaRepository)
    {
        _subastaRepository = subastaRepository;
    }

    public async Task<Subasta?> HandleAsync(ObtenerSubastaPorIdQuery query)
    {
        return await _subastaRepository.GetByIdAsync(query.Id);
    }
}