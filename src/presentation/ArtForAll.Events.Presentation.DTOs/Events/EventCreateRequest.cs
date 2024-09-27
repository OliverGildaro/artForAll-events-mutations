namespace ArtForAll.Events.Presentation.DTOs.Events
{
    using Microsoft.AspNetCore.Http;

    public class EventCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}
