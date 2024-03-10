namespace TherapyApiService.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(DateOnly date);
    }
}