using Application.UseCases.Subasta.Handlers;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers
{
    public class SubastaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubastaWorker> _logger;

        public SubastaWorker(IServiceProvider serviceProvider, ILogger<SubastaWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando Background Worker de Adjudicación de Subastas...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<SubastaDbContext>();
                        var finalizarHandler = scope.ServiceProvider.GetRequiredService<FinalizarSubastaCommandHandler>();

                        // Busca subastas activas cuya fecha_fin ya haya pasado
                        var subastasVencidas = await context.Subasta
                            .Where(s => s.estado == Domain.Enums.EstadoSubasta.ACTIVA && s.fecha_fin <= DateTime.Now)
                            .Select(s => s.id)
                            .ToListAsync(stoppingToken);

                        foreach (var subastaId in subastasVencidas)
                        {
                            _logger.LogInformation($"Ejecutando adjudicación automática para la subasta ID: {subastaId}");
                            await finalizarHandler.HandleAsync(subastaId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando el cierre automático de subastas.");
                }

                // Verifica cada 15 segundos
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }
    }
}