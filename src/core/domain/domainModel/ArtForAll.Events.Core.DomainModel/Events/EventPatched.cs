namespace ArtForAll.Events.Core.DomainModel.Events
{
    using System.Threading.Tasks;
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.Core.DomainModel.Interfaces;

    public class EventPatched : IDomainEvent
    {
        public string State { get; set; }
        public string Name { get; set; }
        public IEnumerable<EventPatchOperation> PatchOperations { get; set; } =new List<EventPatchOperation>();

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
