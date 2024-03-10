
namespace TherapyApiService.Workers
{
    public abstract class SpecificTimedWorker : IHostedService
    {
        private readonly ILogger logger;
        private readonly TimeOnly specificTime;
        private Timer timer;

        public SpecificTimedWorker(
            ILogger logger,
            TimeOnly specificTime)
        {
            this.logger = logger;
            this.specificTime = specificTime;
        }

        public abstract Task DoWork();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            DateTime nowTime = DateTime.UtcNow;
            DateTime dueTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, specificTime.Hour, specificTime.Minute, specificTime.Second, DateTimeKind.Utc);
            if (nowTime > dueTime)
                dueTime = dueTime.AddDays(1);

            timer = new Timer(OnTimer, null, dueTime - nowTime, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Dispose();
            timer = null;

            return Task.CompletedTask;
        }

        private async void OnTimer(object state)
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