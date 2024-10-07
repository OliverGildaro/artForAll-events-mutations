namespace ArtForAll.Events.Core.Commanding.Events.PatchEvent
{
    using ArtForAll.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Core.Commanding.decorators.Auditing;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
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
                Description = @event.Description,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                Type = @event.Type,
                Address = new AddressRequest{
                    City = @event.Address.City,
                    Country = @event.Address.Country,
                    Number = @event.Address.Number,
                    Street = @event.Address.Street,
                    ZipCode = @event.Address.ZipCode    
                },
                Capacity = @event.Capacity,
                Price = new PriceRequest
                {
                    CurrencyExchange = @event.Price.CurrencyExchange,
                    MonetaryValue = @event.Price.MonetaryValue
                },
            };

            command.PatchDocument.ApplyTo(eventRequest);



            List<EventPatchOperation> patchOperations = command.PatchDocument.Operations.Select(op => new EventPatchOperation
            {
                Path = op.path,
                Op = op.op,
                Value = op.value
            }).ToList();
            Result<TypeEvent, Error> typeResult = TypeEvent.CreateNew(eventRequest.Type);
            if (typeResult.IsFailure)
            {
                return Result.Failure(eventResult.Error.Message);
            }
            var (City, Country, Street, Number, ZipCode) = eventRequest.Address;
            var addressResult = Address.CreateNew(City, Country, Street, Number, ZipCode);

            var priceResult = Price.CreateNew(eventRequest.Price.CurrencyExchange, eventRequest.Price.MonetaryValue);

            Result @eventUpdatedResult = @event.Update(
                eventRequest.Description,
                eventRequest.StartDate,
                eventRequest.EndDate,
                typeResult.Value,
                addressResult.Value,
                priceResult.Value,
                patchOperations);

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
