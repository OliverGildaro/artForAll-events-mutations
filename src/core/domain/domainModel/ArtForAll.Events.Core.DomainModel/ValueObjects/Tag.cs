using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Core.DomainModel.ValueObjects
{
    public class Tag : ValueObject
    {
        private string value;
        public string Value => this.value;

        public Tag(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Tag name cannot be null or empty.", nameof(value));
            }

            this.value = value;
        }

        public static Result<Tag, Error> CreateNew(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Errors.General.ValueIsRequired();
            }

            return new Tag(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
