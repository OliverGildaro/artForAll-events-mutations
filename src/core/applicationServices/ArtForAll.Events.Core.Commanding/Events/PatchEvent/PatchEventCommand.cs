namespace ArtForAll.Events.Core.Commanding.Events.PatchEvent
{
    using ArtForAll.Events.Presentation.DTOs.Events;
    using ArtForAll.Shared.Contracts.CQRS;
    using Microsoft.AspNetCore.JsonPatch;

    public class PatchEventCommand : ICommand
    {
        public string EventId { get; set; }
        public JsonPatchDocument<EventPatchRequest> PatchDocument { get; set; }
    }
}
