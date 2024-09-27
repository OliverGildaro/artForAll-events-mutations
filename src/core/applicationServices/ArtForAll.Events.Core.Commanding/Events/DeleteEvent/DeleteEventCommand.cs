using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Core.Commanding.Events.PublishEvent
{
    public class DeleteEventCommand : ICommand
    {
        public string Id { get; set; }
    }
}
