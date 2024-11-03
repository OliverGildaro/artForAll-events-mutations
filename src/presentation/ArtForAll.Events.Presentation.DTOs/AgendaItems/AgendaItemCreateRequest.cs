namespace ArtForAll.Events.Presentation.DTOs.AgendaItems
{
    public class AgendaItemCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan Duration { get; set; }
    }

}
