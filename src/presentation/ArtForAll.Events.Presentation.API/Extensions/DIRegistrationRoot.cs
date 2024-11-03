namespace ArtForAll.Presentation.API.Extensions
{
    using ArtForAll.Events.Core.Commanding.extensions;
    using ArtForAll.Events.Presentation.API.Extensions;
    using ArtForAll.Events.Infrastructure.EFRepository.extensions;
    using ArtForAll.Events.NotificationService.extensions;
    using ArtForAll.Events.S3ImagesBuckets.Extensions;
    using ArtForAll.Events.Presentation.API.Interfaces;
    using ArtForAll.Events.Presentation.API.ExceptionHandlers;
    using ArtForAll.Events.Presentation.API.Utils;

    public static class DIRegistrationRoot
    {
        public static void AddServices(this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddAPIServices();
            services.AddComandHandlers();
            services.AddRepositoryServices(configuration, options => options.UseConsole = environment.IsDevelopment());
            services.AddAmazonNotificationServices();
            services.AddAmazonS3BucketsServices();
            services.AddTransient<IEventMutationsExceptionHandler, EventMutationsExceptionHandler>();
            services.AddTransient<IImageMutationsExceptionHandler, ImageMutationsExceptionHandler>();
        }
    }
    //Dependency inverion and Inversion of control
    //Transient: A new instance is created every time a resource is requested
    //Singleton: One instance is created once and reused from then onwards as long the container exist
    //Scoped: One instance created per scope, an the reused in the scope
}
