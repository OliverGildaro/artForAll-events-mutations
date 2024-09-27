namespace ArtForAll.Events.UnitTests
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
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
                new object[] { "DDD event", "A DDD event to participate", DateTime.UtcNow.AddDays(1), "Poetry", "DRAFT" },
                new object[] { "DDD eveneent", "Description event2", DateTime.UtcNow.AddDays(2), "Music", "DRAFT" },
            };
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]//this member data can be share across many unit tests
        public void WhenCreateANewEvent_ThenItShouldReturANewInstance(
            string name,
            string description,
            DateTime date,
            string type,
            string state)
        {
            var @event = CreateEvent(name, description, date, type, state);
            Assert.Equal(name, @event.Name);
            Assert.Equal(description, @event.Description);
            Assert.Equal(date, @event.Date);
            Assert.Equal(type, @event.Type);
            Assert.Equal(StateEvent.DRAFT, @event.State);
            Assert.Contains("DDD", @event.Name);
            Assert.DoesNotContain("TTT", @event.Name);
            Assert.StartsWith("DDD", @event.Name);
            Assert.EndsWith("ent", @event.Name);
            Assert.Matches("DD(D|E) eve(n|t|k)", @event.Name);
            Assert.True(@event.Date > DateTime.UtcNow.AddDays(0), "Date can not be before than the current date");
            Assert.InRange(@event.Date, DateTime.UtcNow, DateTime.UtcNow.AddYears(100));
            Assert.IsType<Event>(@event);
            Assert.IsAssignableFrom<AggregateRoot>(@event);
            Assert.IsAssignableFrom<Entity>(@event);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenPublishAnEvent_ThenItShouldReturANewInstance(
            string name,
            string description,
            DateTime date,
            string type,
            string state)
        {
            var @event = CreateEvent(name, description, date, type, state);
            var result = @event.Publish();

            Assert.True(result.IsSucces);
            Assert.Equal(StateEvent.PUBLISHED, @event.State);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenPublishAPublishedEvent_ThenItShouldReturAFailureResult(
            string name,
            string description,
            DateTime date,
            string type,
            string state)
        {
            var @event = CreateEvent(name, description, date, type, state);
            var result = @event.Publish();
            var resultRepublished = @event.Publish();
            Assert.True(resultRepublished.IsFailure);
        }

        [Theory]
        [MemberData(nameof(Example_WithMethod))]
        public void WhenDeleteAnEvent_ThenItShouldReturASuccessResult(
            string name,
            string description,
            DateTime date,
            string type,
            string state)
        {
            var @event = CreateEvent(name, description, date, type, state);
            Result res = @event.Delete();
            Assert.True(res.IsSucces);
            Assert.Equal(StateEvent.DELETED, @event.State);
        }

        private Event CreateEvent(string name, string description, DateTime date, string type, string state)
        {
            var typeResult = TypeEvent.CreateNew(type);
            var stateResult = StateEvent.CreateNew(state);
            var @eventResult = Event.CreateNew(name, description, date, typeResult.Value, stateResult.Value);
            return @eventResult.Value;
        }
    }
}
