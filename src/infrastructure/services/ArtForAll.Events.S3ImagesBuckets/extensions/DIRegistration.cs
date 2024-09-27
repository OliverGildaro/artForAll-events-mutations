namespace ArtForAll.Events.S3ImagesBuckets.Extensions
{
    using Amazon.S3;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Events.S3ImagesBuckets.Utils;
    using Microsoft.Extensions.DependencyInjection;

    public static class DIRegistration
    {
        public static IServiceCollection AddAmazonS3BucketsServices(this IServiceCollection services)
        {
            services.AddTransient<IEventImageService, EventImageService>();
            services.AddSingleton<IAmazonS3, AmazonS3Client>();
            services.AddSingleton<IS3BucketsExceptionHandler, S3BucketsExceptionHandler>();
            return services;
        }
    }
}
