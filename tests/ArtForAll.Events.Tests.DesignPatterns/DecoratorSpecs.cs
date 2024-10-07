using ArtForAll.Core.Commanding.Events.CreateEvent;
using ArtForAll.Events.Core.Commanding.decorators.Auditing;
using ArtForAll.Events.Core.DomainModel.Entities;
using ArtForAll.Events.Presentation.DTOs.Events;
using ArtForAll.Events.S3ImagesBuckets.Interfaces;
using ArtForAll.Infrastructure.EFRepositories.Interfaces;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;
using Moq;

namespace ArtForAll.Events.Tests.DesignPatterns
{
    //The intent of this pattern is to attach additional
    //responsabilities to an object dinamically, at run time
    //Decorator and concrete services inherit from the same base interface
    //Decorator receives in the constructor the base interface
    //So we ca inect there the concrete service

    //Use cases
    //Adding loging and monitotring
    //Formattig text in text edittors
    //Adding authentication and authorization layers
    //Enhance the appeareance of UI components


    //Consequences
    //More flexible tha inheritance
    //Respect single responsability
    //Increase effort learing 
    public class DecoratorSpecs
    {
        [Fact]
        public async Task Testing_Decorators()
        {
            //mock repository
            var eventRepository = new Mock<IEventMutationsRepository>();
            eventRepository.Setup(m => m.Insert(It.IsAny<Event>())).Returns(Result.Success);
            eventRepository.Setup(m => m.SaveChangesAsync()).ReturnsAsync(Result.Success);

            //image service mock
            var imageService = new Mock<IEventImageService>();
            imageService.Setup(m => m.UpdateImageAsync(It.IsAny<ImageBuffer>())).ReturnsAsync(Result.Success);

            //concrete handlers
            var handler = new CreateEventCommandHandler(eventRepository.Object, imageService.Object);
            var loggingDecorator = new AuditLoggingDecorator<CreateEventCommand, Result>(handler);
            var createEventCommand = new CreateEventCommand()
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

            var createEventResult = await loggingDecorator.HandleAsync(createEventCommand);

            Assert.True(createEventResult.IsSucces);
            Assert.NotNull(createEventResult.Id);
        }
    }
}