using Application.UseCases.Subasta.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Workers
{
    public class SubastaProgramadaWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubastaProgramadaWorker> _logger;

        public SubastaProgramadaWorker(
            IServiceProvider serviceProvider,
            ILogger<SubastaProgramadaWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando Background Worker de Subastas programadas...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var useCase = scope.ServiceProvider.GetRequiredService<IniciarSubastasProgramadasCommandHandler>();

                    await useCase.ExecuteAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando el inicio automático de subastas.");
                }

                await Task.Delay(TimeSpan.FromSeconds(15),stoppingToken);
            }
        }
    }
}
