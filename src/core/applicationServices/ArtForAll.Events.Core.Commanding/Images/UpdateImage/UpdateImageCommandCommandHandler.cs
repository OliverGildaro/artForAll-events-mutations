namespace ArtForAll.Core.Commanding.Images.AddImage
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Infrastructure.EFRepository;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]
    public class UpdateImageCommandCommandHandler : ICommandHandler<UpdateImageCommand, Result>
    {
        private readonly IEventImageService imageService;
        private readonly IEventMutationsRepository eventMutationsRepository;

        public UpdateImageCommandCommandHandler(IEventImageService imageService, IEventMutationsRepository eventMutationsRepository)
        {
            this.imageService = imageService;
            this.eventMutationsRepository = eventMutationsRepository;
        }

        public async Task<Result> HandleAsync(UpdateImageCommand command)
        {
            var @event = await eventMutationsRepository.FindAsync(command.EventId);
            if (@event.IsFailure || !@event.Value.AllowAddImageIsSuccess() || @event.Value.Image is null)
            {
                return Result.Failure("Not possible to update the image");
            }

            @event.Value.UpdateImage(command.ContentType, command.FileName);

            await eventMutationsRepository.SaveChangesAsync();

            //imageBuffer
            var imageBuffResult = ImageBuffer.CreateNew(command.ImageContent, @event.Value.Image.Id);
            if (imageBuffResult.IsFailure)
            {
                return Result.Failure(imageBuffResult.Error.Message);
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
