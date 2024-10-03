namespace ArtForAll.Events.Presentation.DTOs.Events
{
    using Microsoft.AspNetCore.Http;
    using System.Net;

    public class EventCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public AddressRequest Address { get; set; }
        public int? Capacity { get; set; }
        public PriceRequest Price { get; set; }
    }
}
