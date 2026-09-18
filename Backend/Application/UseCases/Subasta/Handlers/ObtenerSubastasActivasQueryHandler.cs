namespace Application.UseCases.Subasta.Handlers;

using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

public class ObtenerSubastasActivasQueryHandler
{
    private readonly ISubastaRepository _subastaRepository;

    public ObtenerSubastasActivasQueryHandler(ISubastaRepository subastaRepository)
    {
        _subastaRepository = subastaRepository;
    }

    public async Task<IEnumerable<Subasta>> HandleAsync()
    {
        return await _subastaRepository.GetAllActivasAsync();
    }
}