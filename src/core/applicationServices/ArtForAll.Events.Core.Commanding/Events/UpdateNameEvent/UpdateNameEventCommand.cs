namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Shared.Contracts.CQRS;

    public class UpdateNameEventCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
