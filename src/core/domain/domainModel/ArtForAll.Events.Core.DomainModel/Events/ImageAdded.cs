using ArtForAll.Core.DomainModel.Interfaces;
using ArtForAll.Events.Core.DomainModel.Interfaces;

namespace ArtForAll.Events.Core.DomainModel.Events
{
    public class ImageAdded : IDomainEvent
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string EventId { get; set; }
        public string contentType { get; set; }
        public string fileName { get; set; }

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
