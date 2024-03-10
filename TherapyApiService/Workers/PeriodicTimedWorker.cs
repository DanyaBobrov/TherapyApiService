
namespace TherapyApiService.Workers
{
    public abstract class PeriodicTimedWorker : BackgroundService
    {
        private readonly ILogger logger;
        private readonly TimeSpan period;

        public PeriodicTimedWorker(
            ILogger logger,
            TimeSpan period)
        {
            this.logger = logger;
            this.period = period;
        }

        public abstract Task DoWork();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await DoWork();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error while '{nameof(DoWork)}'");
                }
            }
        }
    }
}