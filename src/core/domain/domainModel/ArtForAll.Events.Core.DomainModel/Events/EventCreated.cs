namespace ArtForAll.Events.Core.DomainModel.Events
{
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.DomainModel.Interfaces;

    public class EventCreated : IDomainEvent
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string CreatedAt { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string StateEvent { get; set; }
        public ImageAdded Image { get; set; }

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
