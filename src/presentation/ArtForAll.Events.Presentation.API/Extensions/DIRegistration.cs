using ArtForAll.Presentation.API.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ArtForAll.Events.Presentation.API.Extensions
{
    public static class DIRegistration
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            //If CommandDispatcher would have a contructor DI, then that class will be singleton too regarding the registration type
            //This problem is called dependency captivity
            services.TryAddSingleton<CommandDispatcher>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigins", builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}
