using SortingJobScheduler.Interfaces.Queue;

namespace SortingJobScheduler.API.HostedServices
{
    public class SortingJobHostedService : BackgroundService
    {
        private readonly ISortingJobQueue _queue;
        private readonly ILogger _logger;

        public SortingJobHostedService(
            ISortingJobQueue queue, 
            ILogger<SortingJobHostedService> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"SortingJobHostedService is running.");
            await ProcessWorkItemAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SortingJobHostedService is stopping.");
            await base.StopAsync(stoppingToken);
        }

        private async Task ProcessWorkItemAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var job = await _queue.DequeueAsync(cancellationToken);

                try
                {
                    if (job == null) continue;

                    await job(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occured while executing job");
                }
            }
        }
    }
}
