namespace ArtForAll.Events.Infrastructure.EFRepository.extensions
{
    using ArtForAll.Shared.Interfaces;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using OCP.PortalEvents.Repositories.Context;
    using Microsoft.Extensions.Configuration;
    using ArtForAll.Shared.ExceptionHandler;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Infrastructure.EFRepository.visitors;
    using ArtForAll.Events.Infrastructure.EFRepository.utils;
    using ArtForAll.Events.Infrastructure.EFRepository.Interfaces;

    public static class DIRegistration
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services,
            IConfiguration configuration,
            Action<ConsoleOptions> consoleModifier)
        {
            var options = new ConsoleOptions();
            consoleModifier(options);

            services.AddTransient<IDomainEventVisitor, DomainEventPublisherVisitor>();
            services.AddTransient<IEventRepositoryExceptionHandler, EventRepositoryExceptionHandler>();

            services.AddTransient(provider => new EventsContext(
                configuration.GetConnectionString("conectionDb"),
                options.UseConsole,
                provider.GetRequiredService<IDomainEventVisitor>()));
            services.AddRepositories();

            return services;
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            var repositories = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsClass && type.GetInterfaces().Any(interfaceType => IsRepositoryInterface(interfaceType)))
                .ToList();

            foreach (Type repository in repositories)
            {
                AddRepository(services, repository);
            }
        }

        private static void AddRepository(IServiceCollection services, Type repository)
        {
            var repositoryInterface = repository.GetInterfaces()
                .FirstOrDefault(interfaceType => !interfaceType.IsGenericType);
            if (repositoryInterface == null)
                return;

            //TODO:We should register this as Scoped
            services.AddTransient(repositoryInterface, provider =>
            {
                var repositoryInstance = ActivatorUtilities.CreateInstance(provider, repository);
                return repositoryInstance;
            });
        }

        private static bool IsRepositoryInterface(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMutationsRepository<,,>);
        }
    }
}
