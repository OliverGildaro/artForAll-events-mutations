namespace ArtForAll.Events.UnitTests.Fixtures
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
    using Moq;

    public class PatchEventCommandHandlerFixture
    {
        public readonly PatchEventCommandHandler commandHandler;
        public PatchEventCommandHandlerFixture()
        {
            var eventRepository = new Mock<IEventMutationsRepository>();
            eventRepository.Setup(m => m.FindAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        var result = Event.CreateNew(
                            "Art event",
                            "Art event description",
                            DateTime.UtcNow.AddDays(1),
                            TypeEvent.CreateNew("Music").Value,
                            StateEvent.DRAFT);
                        return result;
                    }
                    else
                    {
                        return Result<Event, Error>.Failure(new Error("Invalid ID", "The provided ID is empty or whitespace."));
                    }
                });
            eventRepository.Setup(m => m.SaveChangesAsync())
                .ReturnsAsync(Result.Success);
            var imageService = new Mock<IEventImageService>();
            imageService.Setup(m => m.UpdateImageAsync(It.IsAny<ImageBuffer>()))
                .ReturnsAsync(Result.Success);
            this.commandHandler = new PatchEventCommandHandler(eventRepository.Object, imageService.Object);
        }

        public void Dispose()
        {
            //clean up if required
        }
    }
}
