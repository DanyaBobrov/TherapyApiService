using Common.MessengerBot.Telegram;
using Common.MessengerBot.Telegram.Models;
using Microsoft.Extensions.Options;
using TherapyApiService.Models;
using TherapyApiService.Models.Options;
using TherapyApiService.Services.Interfaces;

namespace TherapyApiService.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> logger;
        private readonly ITelegramBotFactory telegramBotFactory;
        private readonly IOptionsMonitor<NotificationOptions> optionsMonitor;

        public NotificationService(
            ILogger<NotificationService> logger,
            ITelegramBotFactory telegramBotFactory,
            IOptionsMonitor<NotificationOptions> optionsMonitor)
        {
            this.logger = logger;
            this.telegramBotFactory = telegramBotFactory;
            this.optionsMonitor = optionsMonitor;
        }

        public async Task NotifyAsync(DateOnly date)
        {
            try
            {
                var bot = telegramBotFactory.CreateBot(Constants.Bots.AlertBot);

                string text = "";
                if (DateOnly.FromDateTime(DateTime.Now) >= date)
                    text = string.Format(Constants.Messages.Info, date.ToString("yyyy-MM-dd"));
                else
                    text = string.Format(Constants.Messages.Alert, date.ToString("yyyy-MM-dd"));

                var message = new MessageInfo()
                {
                    ChatId = optionsMonitor.CurrentValue.ChatId,
                    Text = text
                };
                var result = await bot.SendMessageAsync(message);

                logger.LogInformation($"Message {result.MessageId} to chatId {message.ChatId} was sent");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while {nameof(NotifyAsync)}");
            }
        }
    }
}