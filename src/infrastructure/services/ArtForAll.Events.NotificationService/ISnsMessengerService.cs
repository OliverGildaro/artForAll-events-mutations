namespace ArtForAll.Events.NotificationService
{
    using Amazon.SimpleNotificationService.Model;

    public interface ISnsMessengerService
    {
        Task<PublishResponse> PublishAsync<T>(T message);
    }
}
