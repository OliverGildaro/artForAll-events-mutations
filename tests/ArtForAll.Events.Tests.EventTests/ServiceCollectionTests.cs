
namespace ArtForAll.Events.UnitTests
{
    using System;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Infrastructure.EFRepository;
    using ArtForAll.Events.Infrastructure.EFRepository.extensions;
    using ArtForAll.Events.Infrastructure.EFRepository.Interfaces;
    using ArtForAll.Events.Infrastructure.EFRepository.utils;
    using ArtForAll.Events.NotificationService;
    using ArtForAll.Events.NotificationService.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class ServiceCollectionTests
    {
        [Fact]
        public void RegisterDataServices_Execute_DataServicesAreRegistered()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string> {
                        {"ConnectionStrings:EmployeeManagementDB", "AnyValueWillDo"}})
                .Build();

            var mockDomainEventVisitor = new Mock<IDomainEventVisitor>();
            var snsMessenger = new Mock<ISnsMessengerService>();
            var snsExceptionHandler = new Mock<ISnsMessengerExceptionHandler>();
            
            var mockExceptionHandlerBase = new Mock<IEventRepositoryExceptionHandler>();
            var mockEventRepository = new Mock<ILogger<EventRepositoryExceptionHandler>>();


            serviceCollection.AddTransient(provider => mockDomainEventVisitor.Object);
            serviceCollection.AddTransient(provider => mockExceptionHandlerBase.Object);
            serviceCollection.AddTransient(provider => snsMessenger.Object);
            serviceCollection.AddTransient(provider => snsExceptionHandler.Object);
            serviceCollection.AddTransient(provider => mockEventRepository.Object);


            // Act
            serviceCollection.AddRepositoryServices(configuration, options => options.UseConsole = true);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            Assert.NotNull(
                serviceProvider.GetService<IEventMutationsRepository>());
            Assert.IsType<EventMutationsRepository>(
                serviceProvider.GetService<IEventMutationsRepository>());
        }
    }
}
