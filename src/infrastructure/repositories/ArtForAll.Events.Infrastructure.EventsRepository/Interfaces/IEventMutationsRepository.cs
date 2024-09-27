
namespace ArtForAll.Infrastructure.EFRepositories.Interfaces
{
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.Interfaces;

    public interface IEventMutationsRepository : IMutationsRepository<Event, Error, string>
    { }
}