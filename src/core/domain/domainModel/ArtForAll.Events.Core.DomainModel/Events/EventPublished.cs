namespace ArtForAll.Events.Core.DomainModel.Events
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.Interfaces;

    public class EventPublished : IDomainEvent
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string StateEvent { get; set; }

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
