namespace ArtForAll.Events.Presentation.DTOs.Events
{
    public class EventPatchRequest {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public AddressRequest Address { get; set; }
        public int Capacity { get; set; }
        public PriceRequest Price { get; set; }
    }
}
