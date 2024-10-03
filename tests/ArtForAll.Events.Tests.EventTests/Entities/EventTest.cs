namespace ArtForAll.Events.UnitTests
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
    using System.Diagnostics.Metrics;
    using System.IO;
    using System.Reflection.Emit;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class EventTest
    {
        //MS test framework crated for .net framework and open source since .net core2
        //nUnit created for java compatibilities and works in .net
        //xUnit since .net core and new .NET features in mind

        //Fake: A working implementation not suitable for production use
        //Dummy: A test double that's never accessed or used
        //Stub: A test double that provides fake data to the system under test
        //Spy: A test double capable of capturing indirect output and providing indirect input as needed
        //Mock: A test double that implements the expected behavior
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
        [MemberData(nameof(Example_WithMethod))]//this member data can be share across many unit tests
        public void WhenCreateANewEvent_ThenItShouldReturANewInstance(
            string name,
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

            Assert.Equal(name, @event.Name);
            Assert.Equal(description, @event.Description);
            Assert.Equal(startDate, @event.StartDate);
            Assert.Equal(type, @event.Type);
            Assert.Equal(StateEvent.DRAFT, @event.State);
            Assert.Contains("DDD", @event.Name);
            Assert.DoesNotContain("TTT", @event.Name);
            Assert.StartsWith("DDD", @event.Name);
            Assert.EndsWith("ent", @event.Name);
            Assert.Matches("DD(D|E) eve(n|t|k)", @event.Name);
            Assert.True(@event.StartDate > DateTime.UtcNow.AddDays(0), "startDate can not be before than the current startDate");
            Assert.InRange(@event.StartDate, DateTime.UtcNow, DateTime.UtcNow.AddYears(100));
            Assert.IsType<Event>(@event);
            Assert.IsAssignableFrom<AggregateRoot>(@event);
            Assert.IsAssignableFrom<Entity>(@event);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenPublishAnEvent_ThenItShouldReturANewInstance(
            string name,
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

            var result = @event.Publish();
            Assert.True(result.IsSucces);
            Assert.Equal(StateEvent.PUBLISHED, @event.State);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenPublishAPublishedEvent_ThenItShouldReturAFailureResult(
            string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            string type,
            string state, string city,
            string country,
            string street,
            string number,
            string zipCode,
            int? capacity,
            string CurrencyExchange,
            float? monetaryValue)
        { 
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

            var result = @event.Publish();
            var resultRepublished = @event.Publish();
            Assert.True(resultRepublished.IsFailure);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenDeleteAnEvent_ThenItShouldReturASuccessResult(
            string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            string type,
            string state, string city,
            string country,
            string street,
            string number,
            string zipCode,
            int? capacity,
            string CurrencyExchange,
            float? monetaryValue)
        {
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

            Result res = @event.Delete();
            Assert.True(res.IsSucces);
            Assert.Equal(StateEvent.DELETED, @event.State);
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
