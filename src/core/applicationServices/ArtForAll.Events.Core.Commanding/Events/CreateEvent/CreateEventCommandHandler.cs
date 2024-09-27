namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Factories;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]
    public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, Result>
    {
        private readonly IEventMutationsRepository repository;
        private readonly IEventImageService imageService;

        public CreateEventCommandHandler(IEventMutationsRepository repository, IEventImageService imageService)
        {
            this.repository = repository;
            this.imageService = imageService;
        }
        public async Task<Result> HandleAsync(CreateEventCommand command)
        {
            Result<Event, Error> @eventResult = EventFactory.CreateNew(command.Name,
                command.Description,
                command.date,
                command.Type);

            if (@eventResult.IsFailure)
            {
                return Result.Failure(@eventResult.Error.Message);
            }
            var @event = @eventResult.Value;
            var eventContext = repository.Insert(@event);

            if (eventContext.IsFailure)
            {
                return Result.Failure(eventContext.Message);
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
