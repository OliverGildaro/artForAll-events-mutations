namespace ArtForAll.Core.Commanding.Events.CreateEvent
{
    using ArtForAll.Events.Presentation.DTOs.Events;
    using ArtForAll.Shared.Contracts.CQRS;

    public class CreateEventCommand : ICommand
    {
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
