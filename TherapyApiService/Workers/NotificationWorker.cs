
using Microsoft.Extensions.Options;
using TherapyApiService.Models.Options;
using TherapyApiService.Repositories.Interfaces;
using TherapyApiService.Services.Interfaces;

namespace TherapyApiService.Workers
{
    public class NotificationWorker : SpecificTimedWorker
    {
        private static TimeOnly Time = new TimeOnly(16, 00, 00);
        private readonly ILogger<NotificationWorker> logger;
        private readonly IInjectionRepository injectionRepository;
        private readonly INotificationService notificationService;
        private readonly IOptionsMonitor<NotificationOptions> options;

        public NotificationWorker(
            ILogger<NotificationWorker> logger,
            IInjectionRepository injectionRepository,
            INotificationService notificationService,
            IOptionsMonitor<NotificationOptions> options)
            : base(logger, Time)
        {
            this.logger = logger;
            this.injectionRepository = injectionRepository;
            this.notificationService = notificationService;
            this.options = options;
        }

        public override async Task DoWork()
        {
            var injections = await injectionRepository.FindPlannedInjections();

            if (!injections.Any())
            {
                logger.LogInformation("Planned injections not found");
                return;
            }

            var nearestInjection = injections
                .OrderBy(x => x.TargetDate)
                .First();

            logger.LogInformation($"Next nearest injection {nearestInjection.Id} will be on {nearestInjection.TargetDate}");

            var nowDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var countDays = nowDate.DayNumber - nearestInjection.TargetDate.DayNumber;
            if (countDays < options.CurrentValue.Days)
            {
                logger.LogInformation($"{countDays} before the injection. Notify to messenger");

                await notificationService.NotifyAsync(nearestInjection.TargetDate);
            }
        }
    }
}