namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Shared.Contracts.CQRS;

    public class UpdateImageCommand : ICommand
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public byte[] ImageContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
