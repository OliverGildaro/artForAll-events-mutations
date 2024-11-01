namespace ArtForAll.Events.Core.DomainModel.Entities;

using ArtForAll.Core.DomainModel.Interfaces;
using ArtForAll.Core.DomainModel.ValueObjects;
using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
using ArtForAll.Events.Core.DomainModel.Events;
using ArtForAll.Events.Core.DomainModel.ValueObjects;
using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

public class Event : AggregateRoot, IEvent
{
    private string name;
    private string description;
    private int?  capacity;
    private DateTime startDate;
    private DateTime endDate;
    private TypeEvent type;
    private Address address;
    private Image image;
    private StateEvent state;
    private Price price;

    private DateTime createdAt;
    private DateTime updatedAt;

    protected Event() { }

    private Event(string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        int?  capacity,
        TypeEvent type,
        StateEvent state,
        Address address,
        Price price)
    {
        name = name ?? throw new ArgumentNullException(nameof(type), "Parameter cannot be null");
        type = type ?? throw new ArgumentNullException(nameof(type), "Parameter cannot be null");

        this.name = name;
        this.description = description;
        this.startDate = startDate;
        this.endDate = endDate;
        this.createdAt = DateTime.UtcNow;
        this.updatedAt = DateTime.UtcNow;
        this.capacity = capacity;
        this.type = type;
        base.Id = Guid.NewGuid().ToString();
        this.state = state;
        this.address = address;
        this.price = price;
    }

    public string Name => name;
    public int?  Capacity => capacity;
    public string Description => description;
    public DateTime StartDate => startDate;
    public DateTime EndDate => endDate;
    public TypeEvent Type => type;
    public Address Address => address;
    public Price Price => price;
    public StateEvent State => this.state;
    public virtual Image Image => this.image;
    public DateTime CreatedAt => createdAt;
    public DateTime UpdatedAt => updatedAt;

    public static Result<Event, Error> CreateNew(string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        int? capacity,
        TypeEvent type,
        StateEvent state,
        Address address,
        Price price)
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

        if (startDate.ToUniversalTime() < DateTime.UtcNow)
        {
            return new Error("", "");
        }

        if (type is null)
        {
            return new Error("", "");
        }

        if (state is null)
        {
            return new Error("", "");
        }


        return new Event(name, description, startDate, endDate, capacity, type, state, address, price);
    }

    public Result Update(
        string? description,
        DateTime startDate,
        DateTime endDate,
        TypeEvent type,
        Address address,
        Price price,
        List<EventPatchOperation> patchOperations)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure("");
        }

        if (name.Length > 100)
        {
            return Result.Failure("");
        }

        if (description?.Length > 1000)
        {
            return Result.Failure("");
        }

        if (startDate.ToUniversalTime() < DateTime.UtcNow)
        {
            return Result.Failure("");
        }

        AddEventPatchedDomainEvent(patchOperations);
        this.name = name.Trim();
        this.description = description;
        this.startDate = startDate;
        this.endDate = endDate;
        this.type = type;
        this.address = address;
        this.price = price;

        return Result.Success();
    }

    public Result AddImage(Image image)
    {
        if (this.Image != null)
        {
            return Result.Failure("The image alreaady exist");
        }
        var imageAdded = new ImageAdded
        {
            Id = image.Id,
            CreatedAt = this.CreatedAt.ToString("yyyy-MM-dd"),
            EventId = this.Id,
            contentType = image.ContentType,
            fileName = image.FileName
        };

        this.AddDomainEvent(imageAdded);

        this.image = image;
        return Result.Success();
    }

    public Result Publish()
    {
        Tuple<string, string> prevPk = new(this.name, this.state);
        var stateResult = this.state.Publish();
        if (stateResult.IsFailure)
        {
            return Result.Failure("");
        }
        this.state = stateResult.Value;

        var eventPublished = new EventPublished
        {
            PrevPK = prevPk,
            Id = this.Id,
            StartDate = this.StartDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            EndDate = this.EndDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            CreatedAt = this.CreatedAt.ToString("yyyy-MM-dd"),
            Description = this.Description,
            Name = this.Name,
            Type = this.Type,
            StateEvent = this.State,
            Addres = this.Address,
            Capacity = this.Capacity,
            Price = this.Price,
        };
        this.AddDomainEvent(eventPublished);

        return Result.Success();
    }

    public Result Delete()
    {
        var res = this.state.Delete();

        if (res.IsFailure)
        {
            return Result.Failure("");
        }
        this.state = res.Value;
        var eventDeleted = new EventDeleted
        {
            Id = this.Id,
            CreatedAt = this.CreatedAt.ToString("yyyy-MM-dd"),
            StateEvent = this.state
        };
        this.AddDomainEvent(eventDeleted);
        return Result.Success();
    }

    public void AddEventCreatedDomainEvent(EventCreated eventCreted)
    {
        AddDomainEvent(eventCreted);
    }

    public void AddEventPatchedDomainEvent(List<EventPatchOperation> patchOperations)
    {
        var eventPatched = new EventPatched
        {
            State = this.state,
            Name = this.name,
            PatchOperations = patchOperations
        };

        AddDomainEvent(eventPatched);
    }

    public bool AllowAddImageIsSuccess()
    {
        var allowAddImage = true;
        if (this.State.Equals(StateEvent.DELETED))
        {
            allowAddImage = false;
        }

        if (this.StartDate <= DateTime.UtcNow)
        {
            allowAddImage = false;
        }

        return allowAddImage;
    }

    public void UpdateImage(string contentType, string fileName)
    {
        this.Image.Update(contentType, fileName);
    }

    public Result UpdateName(string name)
    {
        Tuple<string,string> prevNameStater = new(this.state, this.name);
        if (String.IsNullOrWhiteSpace(name))
        {
            return Result.Failure("");
        }
        this.name = name;
        var domainNameUpdated = new EventNameUpdated
        {
            PrevPK = prevNameStater,
            Name = this.name,
            State = this.State,
        };

        AddDomainEvent(domainNameUpdated);
        return Result.Success();
    }
}
