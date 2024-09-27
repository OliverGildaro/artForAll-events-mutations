namespace ArtForAll.Events.Core.Commanding.Events.PublishEvent
{
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]//If a command habnler doesn;t have this atribute, the DI will fail
    public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand, Result>
    {
        private readonly IEventMutationsRepository repository;

        public DeleteEventCommandHandler(IEventMutationsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result> HandleAsync(DeleteEventCommand command)
        {
            var @eventResult = await this.repository.FindAsync(command.Id);

            if (@eventResult.IsFailure || @eventResult.Value.State.Equals(StateEvent.DELETED))
            {
                return Result.Failure("");
            }

            var @event = @eventResult.Value;
            var publishResult = @event.Delete();


            if (publishResult.IsFailure)
            {
                return Result.Failure("");
            }

            var saveResult = await this.repository.SaveChangesAsync();

            if (saveResult.IsFailure)
            {
                return Result.Failure("");
            }

            return Result.Success();
        }
    }
}
