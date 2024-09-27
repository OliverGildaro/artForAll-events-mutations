namespace ArtForAll.Events.Core.DomainModel.Factories
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public class EventFactory
    {
        public static Result<Event, Error> CreateNew(string name, string description, DateTime date, string typeString)
        {
            var type = TypeEvent.CreateNew(typeString);
            if (type.IsFailure)
            {
                return Result<Event, Error>.Failure(type.Error);
            }

            var stateEvent = StateEvent.CreateNew("DRAFT");
            if (stateEvent.IsFailure)
            {
                return Result<Event, Error>.Failure(stateEvent.Error);
            }

            //event created
            var @event = Event.CreateNew(name, description, date, type.Value, stateEvent.Value);
            if (@event.IsFailure)
            {
                return Result<Event, Error>.Failure(@event.Error);
            }
            var eventCreated = @event.Value;

            var eventCreted = new EventCreated
            {
                Id = eventCreated.Id,
                Date = eventCreated.Date.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                CreatedAt = eventCreated.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Description = eventCreated.Description,
                Name = eventCreated.Name,
                Type = eventCreated.Type,
                StateEvent = eventCreated.State,
            };

            eventCreated.AddEventCreatedDomainEvent(eventCreted);

            return Result<Event, Error>.Success(@event.Value);
        }
    }
}
