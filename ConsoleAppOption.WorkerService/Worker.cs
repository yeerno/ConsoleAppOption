using ConsoleAppOption.Processor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleAppOption.WorkerService
{
    public class Worker : IHostedService
    {
        private readonly IHostApplicationLifetime hostLifetime;
        private readonly ILogger<Worker> logger;
        private readonly IMainProcessor mainProcessor;
        private readonly PeriodicTimer timer;

        public Worker(IHostApplicationLifetime hostLifetime, ILogger<Worker> logger, IMainProcessor mainProcessor)
        {
            this.hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mainProcessor = mainProcessor ?? throw new ArgumentNullException(nameof(mainProcessor));
            this.timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            hostLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        logger.LogInformation($"Process Start");

                        while (await timer.WaitForNextTickAsync(cancellationToken))
                        {
                            logger.LogInformation($"Worker is working. Time: {DateTime.Now.ToString("O")}");
                            await mainProcessor.StartProcess(1);
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        logger.LogError(ex, $"Operation Canceled");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error");
                    }
                    finally
                    {
                        hostLifetime.StopApplication();
                    }
                });

            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger?.LogInformation($"Shutting down application.");
            return Task.CompletedTask;
        }
    }
}
