namespace ArtForAll.Events.UnitTests
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public class ImplicitConversions
    {
        [Fact]
        public void WhenItCreatesANewEvent_ThenItShouldImplicitlyConverted()
        {
            Result<TypeEvent, Error> type = TypeEvent.CreateNew("SOLID");
            Assert.IsType<Result<TypeEvent, Error>>(type);
        }
    }
}
