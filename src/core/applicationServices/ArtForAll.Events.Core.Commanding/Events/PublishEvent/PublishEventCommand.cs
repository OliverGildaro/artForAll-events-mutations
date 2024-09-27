using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Core.Commanding.Events.PublishEvent
{
    public class PublishEventCommand : ICommand
    {
        public string Id { get; set; }
    }
}
