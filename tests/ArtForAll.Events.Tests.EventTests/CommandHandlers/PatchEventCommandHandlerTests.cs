namespace ArtForAll.Events.UnitTests.CommandHandlers
{
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.UnitTests.Fixtures;
    using ArtForAll.Events.UnitTests.TestData;

    public class PatchEventCommandHandlerTests : IClassFixture<PatchEventCommandHandlerFixture>
    {
        private readonly PatchEventCommandHandlerFixture commandHandlerFixture;
        public PatchEventCommandHandlerTests(PatchEventCommandHandlerFixture commandHandlerFixture)
        {
            this.commandHandlerFixture = commandHandlerFixture;
        }

        //THEORY WITH CLASS DATA (can be shared across many test classes)
        [Theory]
        [ClassData(typeof(PatchEventCommandHandlerTestData))]
        public async Task WhenCommandHandlerCreateEvent_ThenIsShouldPersistData(PatchEventCommand command)
        {
            //Arrange

            //Act
            var createEventResult = await commandHandlerFixture.commandHandler.HandleAsync(command);

            if (string.IsNullOrEmpty(command.EventId))
            {
                // Assert
                Assert.False(createEventResult.IsSucces);
                Assert.Null(createEventResult.Id);
            }
            else
            {
                // Assert
                Assert.True(createEventResult.IsSucces);
                Assert.NotNull(createEventResult.Id);
            }
        }
    }
}
