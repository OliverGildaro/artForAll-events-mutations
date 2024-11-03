using ArtForAll.Shared.Contracts.CQRS;

namespace ArtForAll.Events.Core.Commanding.AgendaItems
{
    public class AddAgendaItemCommand : ICommand
    {
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
