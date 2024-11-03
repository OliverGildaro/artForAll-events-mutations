namespace ArtForAll.Events.NotificationService.extensions
{
    using Amazon.SimpleNotificationService;
    using ArtForAll.Events.NotificationService.Interfaces;
    using ArtForAll.Events.NotificationService.Utils;
    using Microsoft.Extensions.DependencyInjection;

    public static class DIRegistration
    {
        public static IServiceCollection AddAmazonNotificationServices(this IServiceCollection services)
        {
            services.AddSingleton<ISnsMessengerService, SnsMessengerService>();
            services.AddSingleton<ISnsMessengerExceptionHandler, SnsMessengerExceptionHandler>();
            //services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
            //{
            //    var sqsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.SAEast1);
            //    return sqsClient;
            //});

            return services;
        }
    }
}
