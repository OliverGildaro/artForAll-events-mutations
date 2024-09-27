namespace ArtForAll.Events.UnitTests.CommandHandlers
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.UnitTests.Fixtures;
    using ArtForAll.Events.UnitTests.TestData;

    //SHARING CONTEXT WITH THE CLASS FIXTURE APPROACH
    public class CreateEventCommandHandlerTests : IClassFixture<CreateEventCommandHandlerFixture>
    {
        private readonly CreateEventCommandHandlerFixture commandHandlerFixture;
        public CreateEventCommandHandlerTests(CreateEventCommandHandlerFixture commandHandlerFixture)
        {
            this.commandHandlerFixture = commandHandlerFixture;
        }

        //THEORY WITH CLASS DATA (can be shared across many test classes)
        [Theory]
        [ClassData(typeof(CreateEventCommandHandlerTestData))]
        public async Task WhenCommandHandlerCreateEvent_ThenIsShouldPersistData(CreateEventCommand command)
        {
            //Arrange

            //Act
            var createEventResult = await commandHandlerFixture.commandHandler.HandleAsync(command);

            // Assert
            if (string.IsNullOrEmpty(command.Name) || string.IsNullOrEmpty(command.Type))
            {
                Assert.False(createEventResult.IsSucces);
                Assert.Null(createEventResult.Id);
            }
            else
            {
                // For valid cases, adjust the assertions based on your requirements
                Assert.True(createEventResult.IsSucces);
                Assert.NotNull(createEventResult.Id);
            }
        }
    }
}
