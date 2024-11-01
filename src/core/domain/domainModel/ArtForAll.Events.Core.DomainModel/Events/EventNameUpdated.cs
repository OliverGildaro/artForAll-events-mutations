namespace ArtForAll.Events.Core.DomainModel.Events;

using ArtForAll.Core.DomainModel.Interfaces;
using ArtForAll.Events.Core.DomainModel.Interfaces;
using ArtForAll.Events.Core.DomainModel.ValueObjects;

//DOmain events can contain only primitive types and value objects
public class EventNameUpdated : IDomainEvent
{
    public string Name { get; set; }
    public string State { get; set; }
    public Tuple<string, string> PrevPK { get; set; }

    public async Task Accept(IDomainEventVisitor visitor)
    {
        await visitor.Visit(this);
    }
}
