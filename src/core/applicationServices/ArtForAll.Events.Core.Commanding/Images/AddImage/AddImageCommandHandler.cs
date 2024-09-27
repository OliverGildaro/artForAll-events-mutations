namespace ArtForAll.Core.Commanding.Images.AddImage
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Infrastructure.EFRepository;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;
    using Microsoft.Extensions.Logging;

    [AuditLog]
    public class AddImageCommandHandler : ICommandHandler<AddImageCommand, Result>
    {
        private readonly IEventImageService imageService;
        private readonly IEventMutationsRepository eventMutationsRepository;

        public AddImageCommandHandler(IEventImageService imageService, IEventMutationsRepository eventMutationsRepository)
        {
            this.imageService = imageService;
            this.eventMutationsRepository = eventMutationsRepository;
        }

        public async Task<Result> HandleAsync(AddImageCommand command)
        {
            var @event = await eventMutationsRepository.FindAsync(command.Id);
            if (@event.IsFailure || !@event.Value.AllowAddImageIsSuccess())
            {
                return Result.Failure("");
            }
            //imageCreated
            var imageResult = Image.CreateNew(command.Id, command.ContentType, command.FileName);
            if (imageResult.IsFailure)
            {
                return Result.Failure(imageResult.Error.Message);
            }

            var resultAdd = @event.Value.AddImage(imageResult.Value);

            if (resultAdd.IsFailure)
            {
                return resultAdd;
            }

            await eventMutationsRepository.SaveChangesAsync();

            //imageBuffer
            var imageBuffResult = ImageBuffer.CreateNew(command.ImageContent, imageResult.Value.Id);
            if (imageResult.IsFailure)
            {
                return Result.Failure(imageResult.Error.Message);
            }

            var uploadResult = await this.imageService.UploadImageAsync(imageBuffResult.Value);


            if (uploadResult.IsFailure)
            {
                return Result.Failure(uploadResult.Message);
            }

            return Result.Success(uploadResult.Id);
        }
    }
}
