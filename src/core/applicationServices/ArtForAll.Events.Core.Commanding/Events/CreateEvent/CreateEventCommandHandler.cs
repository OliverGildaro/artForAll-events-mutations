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
            //Create value objects
            var typeResult = TypeEvent.CreateNew(command.Type);
            if (typeResult.IsFailure)
            {
                return Result.Failure("");
            }

            var stateResult = StateEvent.CreateNew("DRAFT");
            if (stateResult.IsFailure)
            {
            }

            var (City, Country, Street, Number, ZipCode) = command.Address;

            var addressResult = Address.CreateNew(City, Country, Street, Number, ZipCode);
            if (addressResult.IsFailure)
            {
            }

            var priceResult = Price.CreateNew(command.Price.CurrencyExchange, command.Price.MonetaryValue);
            if (priceResult.IsFailure)
            {
            }

            //CreateEvent Event entity
            var @eventResult = Event.CreateNew(
                command.Name,
                command.Description,
                command.StartDate,
                command.EndDate,
                command.Capacity,
                typeResult.Value,
                stateResult.Value,
                addressResult.Value,
                priceResult.Value);

            if (@eventResult.IsFailure)
            {
                return Result.Failure("");
            }
            var eventCreated = @eventResult.Value;

            //Adding domain publishing event
            var domainCreatedEvent = new EventCreated
            {
                Id = eventCreated.Id,
                StartDate = eventCreated.StartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                EndDate = eventCreated.EndDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                CreatedAt = eventCreated.CreatedAt.ToString("yyyy-MM-dd"),
                Description = eventCreated.Description,
                Name = eventCreated.Name,
                Type = eventCreated.Type,
                StateEvent = eventCreated.State,
                Addres = eventCreated.Address,
                Capacity = eventCreated.Capacity,
                Price = eventCreated.Price,
            };

            eventCreated.AddEventCreatedDomainEvent(domainCreatedEvent);

            if (@eventResult.IsFailure)
            {
                return Result.Failure(@eventResult.Error.Message);
            }
            var @event = @eventResult.Value;

            //Persisting data
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
