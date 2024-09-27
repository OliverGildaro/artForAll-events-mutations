namespace ArtForAll.Events.Core.Commanding.extensions
{
    using System.Reflection;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Shared.Contracts.CQRS;
    using Microsoft.Extensions.DependencyInjection;

    public static class DIRegistration
    {
        public static IServiceCollection AddComandHandlers(this IServiceCollection services)
        {
            //  Find handler types
            var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType => IsHandlerInterface(interfaceType)))
                .Where(type => type.GetCustomAttributes<AuditLogAttribute>(false).Any())
                .ToList();

            foreach (Type handlerType in handlerTypes)
            {
                AddHandlerWithDecorator(services, handlerType);
            }
            return services;
        }

        private static bool IsHandlerInterface(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICommandHandler<,>);
        }

        private static void AddHandlerWithDecorator(IServiceCollection services, Type handlerType)
        {
            // Get the interface the handler implements
            var handlerInterface = handlerType.GetInterfaces().FirstOrDefault(IsHandlerInterface);
            if (handlerInterface == null)
                return;

            // Register the handler with the decorators
            //TODO:We should register this as Scoped
            services.AddTransient(handlerInterface, provider =>
            {
                var handlerInstance = ActivatorUtilities.CreateInstance(provider, handlerType);
                var decoratorTypes = GetDecoratorTypes(handlerType);

                // Wrap the handler with decorators
                foreach (var decoratorType in decoratorTypes)
                {
                    var genericDecoratorType = decoratorType.MakeGenericType(handlerInterface.GetGenericArguments());
                    handlerInstance = Activator.CreateInstance(genericDecoratorType, handlerInstance);
                }

                return handlerInstance;
            });
        }

        private static List<Type> GetDecoratorTypes(Type handlerType)
        {
            var decorators = new List<Type>();

            if (handlerType.GetCustomAttributes<AuditLogAttribute>(false).Any())
            {
                decorators.Add(typeof(AuditLoggingDecorator<,>));
            }

            // Add more decorators as needed

            return decorators;
        }
        //reflection use cases:
        //*dependency injection
        //*Calling private or protected methods
        //*Serialization
        //*Type inspector applications
        //*Code analysis tools
        //*Create reusable libraries

        //Refelction drawbacks
        //*Reflection is relative slow
        //*Security is merely a suggestion
        //*Reflection is error prone
        //*Working with reflexion is complex
    }
}
