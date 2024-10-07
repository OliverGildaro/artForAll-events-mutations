namespace ArtForAll.Events.Core.DomainModel.Interfaces
{
    using ArtForAll.Events.Core.DomainModel.Events;
    public interface IDomainEventVisitor
    {
        Task Visit(EventCreated eventCreated);
        Task Visit(EventNameUpdated eventUpdated);
        Task Visit(EventPatched eventPatched);
        Task Visit(EventPublished eventPublished);
        Task Visit(EventDeleted eventPublished);
        Task Visit(ImageAdded imagePublished);
    }
}
