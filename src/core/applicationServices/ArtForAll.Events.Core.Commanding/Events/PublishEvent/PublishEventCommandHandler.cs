namespace ArtForAll.Events.Core.Commanding.Events.PublishEvent
{
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.CQRS;
    using ArtForAll.Shared.ErrorHandler;

    [AuditLog]//If a command habnler doesn;t have this atribute, the DI will fail
    public class PublishEventCommandHandler : ICommandHandler<PublishEventCommand, Result>
    {
        private readonly IEventMutationsRepository repository;

        public PublishEventCommandHandler(IEventMutationsRepository repository)
        {
            this.repository = repository;
        }
        public async Task<Result> HandleAsync(PublishEventCommand command)
        {
            var @eventResult = await this.repository.FindAsync(command.Id);

            if (@eventResult.IsFailure)
            {
                return Result.Failure("");
            }

            var @event = @eventResult.Value;
            var publishResult = @event.Publish();


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
