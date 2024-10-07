using ArtForAll.Core.Commanding.Events.CreateEvent;
using ArtForAll.Events.Presentation.DTOs.Events;
using ArtForAll.Infrastructure.EFRepositories.Interfaces;
using ArtForAll.Presentation.API.Utils;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;
using Moq;

namespace ArtForAll.Events.Tests.DesignPatterns
{
    //MEdiator encapsulates how a set of objects interact.
    //Forcing object to communicate via the mediator
    public class MediatorSpecs
    {
        [Fact]
        public async Task Testing_Mediator()
        {
            var command = new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = "Music",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            };

            var serviceProv = new Mock<IServiceProvider>();

            var commadHandler = new Mock<ICommandHandler<CreateEventCommand, Result>>();

            commadHandler.Setup(ch => ch.HandleAsync(command))
                .ReturnsAsync(Result.Success);

            serviceProv.Setup(sp => sp.GetService(typeof(ICommandHandler<CreateEventCommand, Result>)))
                               .Returns(commadHandler.Object);

            CommandDispatcher dispatcher = new(serviceProv.Object);

            var result = await dispatcher.Dispatch(command);
            Assert.True(result.IsSucces);
        }
    }
}
