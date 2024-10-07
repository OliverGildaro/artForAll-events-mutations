namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]
    public class UpdateNameEventCommandHandler : ICommandHandler<UpdateNameEventCommand, Result>
    {
        private readonly IEventMutationsRepository repository;

        public UpdateNameEventCommandHandler(IEventMutationsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result> HandleAsync(UpdateNameEventCommand command)
        {
            var eventResult = await this.repository.FindAsync(command.Id);

            if (eventResult.IsFailure)
            {
                return Result.Failure(eventResult.Error.Message);
            }

            var @eventUpdated = eventResult.Value.UpdateName(command.Name);

            if (@eventUpdated.IsFailure)
            {
                return Result.Failure("");
            }

            var eventInserted = await repository.SaveChangesAsync();

            if (eventInserted.IsFailure)
            {
                return eventInserted;
            }

            return Result.Success();
        }
    }
}
