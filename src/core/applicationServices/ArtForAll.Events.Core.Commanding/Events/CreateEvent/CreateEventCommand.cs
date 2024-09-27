namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Shared.Contracts.CQRS;

    public class CreateEventCommand : ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime date { get; set; }
        public string Type { get; set; }
    }
}
