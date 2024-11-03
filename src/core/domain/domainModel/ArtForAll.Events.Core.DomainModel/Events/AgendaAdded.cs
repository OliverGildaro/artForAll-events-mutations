using ArtForAll.Core.DomainModel.Interfaces;
using ArtForAll.Events.Core.DomainModel.Interfaces;

namespace ArtForAll.Events.Core.DomainModel.Events
{
    public class AgendaAdded : IDomainEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan Duration { get; set; }

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
