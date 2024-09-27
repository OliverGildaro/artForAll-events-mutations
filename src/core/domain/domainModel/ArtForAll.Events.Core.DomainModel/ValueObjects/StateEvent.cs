namespace ArtForAll.Events.Core.DomainModel.ValueObjects
{
    using System.Net;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public class StateEvent : ValueObject
    {
        private string value;
        public static StateEvent DRAFT { get; private set; } = new StateEvent("Draft");
        public static StateEvent PUBLISHED { get; private set; } = new StateEvent("Published");
        public static StateEvent DELETED { get; private set; } = new StateEvent("DELETED");
        protected StateEvent() { }

        private StateEvent(string value)
        {
            value = value ?? throw new ArgumentNullException(nameof(value), "Parameter cannot be null");
            this.value = value.ToUpper();
        }

        public string Value => this.value;

        public static Result<StateEvent, Error> CreateNew(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Errors.General.ValueIsRequired();
            }

            value = value.ToUpper();

            return new StateEvent(value);
        }

        public Result<StateEvent, Error> Publish()
        {
            if (this.value != StateEnum.DRAFT.ToString())
            {
                return new Error($"The value is a not a valid {typeof(StateEnum)}", "StateEnum.value");
            }
            return new StateEvent(StateEnum.PUBLISHED.ToString());
        }

        public static implicit operator string(StateEvent stateEvent)
        {
            return stateEvent.Value;
        }

        public static implicit operator StateEvent(string value)
        {
            return new StateEvent(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private bool IsOnGoing()
        {
            return this.value == StateEnum.ONGOING.ToString();
        }

        internal Result<StateEvent, Error> Delete()
        {
            if (this.IsOnGoing())
            {
                return Result<StateEvent, Error>.Failure(new Error("", ""));
            }
            
            return Result<StateEvent, Error>.Success(new StateEvent("DELETED"));
        }

        private enum StateEnum
        {
            DRAFT,
            PUBLISHED,
            ONGOING,
            COMPLETED,
            DELETED
        }
    }
}
