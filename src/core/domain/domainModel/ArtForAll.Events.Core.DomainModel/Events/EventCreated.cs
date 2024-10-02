namespace ArtForAll.Events.Core.DomainModel.Events
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;

    //DOmain events can contain only primitive types and value objects
    public class EventCreated : IDomainEvent
    {
        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreatedAt { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string StateEvent { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public Address Addres { get; set; }
        public Price Price { get; set; }

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
