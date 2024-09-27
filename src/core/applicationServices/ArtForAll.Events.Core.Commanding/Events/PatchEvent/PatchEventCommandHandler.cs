namespace ArtForAll.Events.Core.Commanding.Events.PatchEvent
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Presentation.DTOs.Events;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]
    public class PatchEventCommandHandler : ICommandHandler<PatchEventCommand, Result>
    {
        private readonly IEventMutationsRepository repository;
        private readonly IEventImageService imageService;

        public PatchEventCommandHandler(IEventMutationsRepository repository, IEventImageService imageService)
        {
            this.repository = repository;
            this.imageService = imageService;
        }
        public async Task<Result> HandleAsync(PatchEventCommand command)
        {
            var eventResult = await this.repository.FindAsync(command.EventId);
            
            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error.Message);
            }

            var @event = eventResult.Value;
            var eventRequest = new EventPatchRequest{
                Name = @event.Name,
                Description = @event.Description,
                date = @event.Date,
                Type = @event.Type
            };

            command.PatchDocument.ApplyTo(eventRequest);

            Result<TypeEvent, Error> typeResult = TypeEvent.CreateNew(eventRequest.Type);

            if (typeResult.IsFailure)
            {
                return Result.Failure(eventResult.Error.Message);
            }

            List<EventPatchOperation> patchOperations = command.PatchDocument.Operations.Select(op => new EventPatchOperation
            {
                Path = op.path,
                Op = op.op,
                Value = op.value
            }).ToList();

            Result @eventUpdatedResult = @event.Update(eventRequest.Name,
                eventRequest.Description,
                eventRequest.date,
                typeResult.Value, patchOperations);

            if (@eventUpdatedResult.IsFailure)
            {
                return Result.Failure(@eventUpdatedResult.Message);
            }


            var eventInserted = await repository.SaveChangesAsync();

            if (eventInserted.IsFailure)
            {
                return eventInserted;
            }

            return Result.Success(@event.Id);
        }
    }
}
