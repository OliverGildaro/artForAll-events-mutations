namespace ArtForAll.Events.UnitTests.Fixtures
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.ErrorHandler;
    using Moq;

    public class CreateEventCommandHandlerFixture : IDisposable
    {
        public readonly CreateEventCommandHandler commandHandler;
        public CreateEventCommandHandlerFixture()
        {
            var eventRepository = new Mock<IEventMutationsRepository>();
            eventRepository.Setup(m => m.Insert(It.IsAny<Event>())).Returns(Result.Success);
            eventRepository.Setup(m => m.SaveChangesAsync()).ReturnsAsync(Result.Success);
            var imageService = new Mock<IEventImageService>();
            imageService.Setup(m => m.UpdateImageAsync(It.IsAny<ImageBuffer>())).ReturnsAsync(Result.Success);
            this.commandHandler = new CreateEventCommandHandler(eventRepository.Object, imageService.Object);
        }

        public void Dispose()
        {
            //clean up if required
        }
    }
}
