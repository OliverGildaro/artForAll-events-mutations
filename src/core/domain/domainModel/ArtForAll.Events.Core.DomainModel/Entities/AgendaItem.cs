using ArtForAll.Core.DomainModel.ValueObjects;
using ArtForAll.Events.Core.DomainModel.ValueObjects;
using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Core.DomainModel.Entities;

public class AgendaItem : Entity
{
    public string Name { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public TimeSpan Duration { get; private set; }
    public string Description { get; private set; }

    protected AgendaItem() { }

    private AgendaItem(string title, DateTime scheduledDate, TimeSpan duration, string description)
    {
        this.Id = Guid.NewGuid().ToString();
        Name = title;
        ScheduledDate = scheduledDate;
        Duration = duration;
        Description = description;
    }

    public static Result<AgendaItem, Error> CreateNew(string name,
    string description,
    DateTime scheduledDate,
    TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new Error("", "");
        }

        if (name.Length > 100)
        {
            return new Error("", "");
        }

        if (description?.Length > 1000)
        {
            return new Error("", "");
        }

        if (scheduledDate.ToUniversalTime() < DateTime.UtcNow)
        {
            return new Error("", "");
        }

        return new AgendaItem(name, scheduledDate, duration, description);
    }
}
