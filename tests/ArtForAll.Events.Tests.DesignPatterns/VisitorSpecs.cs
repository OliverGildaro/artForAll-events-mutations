namespace ArtForAll.Events.Tests.DesignPatterns
{
    using Amazon.SimpleNotificationService.Model;
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Infrastructure.EFRepository.visitors;
    using ArtForAll.Events.NotificationService;
    using ArtForAll.Events.NotificationService.Interfaces;
    using Microsoft.Extensions.FileSystemGlobbing.Internal;
    using Moq;

    //Visitor lets you define a new operation without changing
    // the classes of the elements on which it operates

    //USE CASES
    //When you want to add new behavior or operations to a class hierarchy without
    //modifying the original class definitions, the Visitor pattern can be useful.
    
    //This allows you to keep your classes closed for modification (following the Open/Closed Principle).
    //When you need to perform operations on objects of different types in a collection,
    
    //the Visitor pattern allows you to avoid type-checking or casting.
    //Data structure serailization
    //Document element processing (xml, html)


    //COnsecuencies
    //It may require to break encapsulation]
    //Good to achieve single responsability
    public class VisitorSpecs
    {

        public static IEnumerable<object[]> Example_WithMethod()//since is static can be shared across diferent test clases
        {
            return new List<object[]>
            {
                new object[]
                {
                    "DDD event",
                    "A DDD event to participate",
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(2),
                    "Poetry",
                    "DRAFT" ,
                    "Cochabamba",
                    "Bolivia",
                    "Bartolome",
                    "045",
                    "0000",
                    100,
                    "$",
                    14.55f
                },
                new object[]
                {
                    "DDD eveneent",
                    "Description event2",
                    DateTime.UtcNow.AddDays(2),
                    DateTime.UtcNow.AddDays(3),
                    "Music",
                    "DRAFT",
                    "Cochabamba",
                    "Bolivia",
                    "Bartolome",
                    "045",
                    "0000",
                    1000,
                    "$",
                    14.55f
                },
            };
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public async Task Testing_Decorators(string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            string type,
            string state,
            string city,
            string country,
            string street,
            string number,
            string zipCode,
            int? capacity,
            string CurrencyExchange,
            float? monetaryValue)
        {
            //Mock sns
            var snsService = new Mock<ISnsMessengerService>();
            snsService.Setup(m => m.PublishAsync(It.IsAny<Event>())).ReturnsAsync(new PublishResponse
            {
                MessageId = "12345",
                HttpStatusCode = System.Net.HttpStatusCode.OK
            });

            //Mock exceptio handler
            var exceptionHandler = new Mock<ISnsMessengerExceptionHandler>();
            exceptionHandler.Setup(m => m.HandleAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async (action) =>
                {
                    try
                    {
                        await action(); 
                    }
                    catch (Exception ex)
                    {
                    }
                });


            //mock visitor
            IDomainEventVisitor eventVisitor = new DomainEventPublisherVisitor(snsService.Object, exceptionHandler.Object);

            var domainCreatedEvent = new EventCreated
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1).ToString(),
                EndDate = DateTime.UtcNow.AddDays(2).ToString(),
                Type = "Music",
                Addres = Address.CreateNew("Cochabamba", "Bolivia", "Bartolome c", "088", "0000").Value,
                Capacity = 100,
            };

            var @event = CreateEvent(name,
                description,
                startDate,
                endDate,
                type, state,
                city, country,
                street, number,
                zipCode, capacity,
                CurrencyExchange,
                monetaryValue);

            @event.AddEventCreatedDomainEvent(domainCreatedEvent);

            foreach (IDomainEvent domainEvent in @event.DomainEvents)
            {
                await domainEvent.Accept(eventVisitor);
                Assert.IsType<EventCreated>(domainEvent);
            }
        }

        private Event CreateEvent(string name, string description, DateTime startDate, DateTime endDate, string type, string state, string city,
            string country,
            string street,
            string number,
            string zipCode,
            int? capacity,
            string CurrencyExchange,
            float? monetaryValue)
                {
            var typeResult = TypeEvent.CreateNew(type);
            var stateResult = StateEvent.CreateNew(state);
            var addressResult = Address.CreateNew(city, country, street, number, zipCode);
            var priceResult = Price.CreateNew(CurrencyExchange, monetaryValue);
            var @eventResult = Event.CreateNew(
                name,
                description,
                startDate,
                endDate,
                capacity,
                typeResult.Value,
                stateResult.Value,
                addressResult.Value,
                priceResult.Value);
            return @eventResult.Value;
        }
    }
}
