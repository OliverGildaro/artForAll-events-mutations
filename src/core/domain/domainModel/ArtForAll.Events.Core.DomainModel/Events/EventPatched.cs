namespace ArtForAll.Events.Core.DomainModel.Events
{
    using System.Threading.Tasks;
    using ArtForAll.Core.DomainModel.Interfaces;
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.Core.DomainModel.Interfaces;

    public class EventPatched : IDomainEvent
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public IEnumerable<EventPatchOperation> PatchOperations { get; set; } =new List<EventPatchOperation>();

        public async Task Accept(IDomainEventVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
