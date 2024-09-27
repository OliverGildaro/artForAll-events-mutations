namespace ArtForAll.Core.DomainModel.Interfaces
{
    using ArtForAll.Events.Core.DomainModel.Interfaces;

    public interface IDomainEvent
    {
        Task Accept(IDomainEventVisitor visitor);
    }
}
