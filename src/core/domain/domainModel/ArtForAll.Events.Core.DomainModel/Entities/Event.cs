namespace ArtForAll.Events.Core.DomainModel.Entities
{
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
        private StateEvent state;
        private DateTime date;
        private TypeEvent type;
        private Image image;
        private DateTime createdAt;

        protected Event() { }

        private Event(string name, string description, DateTime date, TypeEvent type, StateEvent state)
        {
            name = name ?? throw new ArgumentNullException(nameof(type), "Parameter cannot be null");
            type = type ?? throw new ArgumentNullException(nameof(type), "Parameter cannot be null");

            this.name = name;
            this.description = description;
            this.date = date;
            this.createdAt = DateTime.UtcNow;
            this.type = type;
            base.Id = Guid.NewGuid().ToString();
            this.state = state;
        }

        public string Name => name;
        public string Description => description;
        public DateTime Date => date;
        public DateTime CreatedAt => createdAt;
        public TypeEvent Type => type;
        public StateEvent State => this.state;
        public virtual Image Image => this.image;

        public static Result<Event, Error> CreateNew(string name, string description, DateTime date, TypeEvent type, StateEvent state)
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

            if (date.ToUniversalTime() < DateTime.UtcNow)
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


            return new Event(name, description, date, type, state);
        }

        public Result Update(string name, string? description, DateTime date, TypeEvent type, List<EventPatchOperation> patchOperations)
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

            if (date.ToUniversalTime() < DateTime.UtcNow)
            {
                return Result.Failure("");
            }

            AddEventPatchedDomainEvent(patchOperations);
            this.name = name.Trim();
            this.description = description;
            this.date = date;
            this.type = type;

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
                CreatedAt = this.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
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
            var stateResult = this.state.Publish();
            if (stateResult.IsFailure)
            {
                return Result.Failure("");
            }
            this.state = stateResult.Value;

            var eventPublished = new EventPublished
            {
                Id = this.Id,
                CreatedAt = this.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                StateEvent = this.state
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
                CreatedAt = this.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
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
                Id = this.Id,
                CreatedAt = this.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
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

            if (this.Date <= DateTime.UtcNow)
            {
                allowAddImage = false;
            }

            return allowAddImage;
        }

        public void UpdateImage(string contentType, string fileName)
        {
            this.Image.Update(contentType, fileName);
        }
    }
}
