namespace ArtForAll.Core.DomainModel.ValueObjects;

using System.Collections.Generic;
using ArtForAll.Generators;
using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

[GenerateToString]
public partial class TypeEvent : ValueObject
{
    private string value;
    protected TypeEvent() { }

    private TypeEvent(string value)
    {
        value = value ?? throw new ArgumentNullException(nameof(value), "Parameter cannot be null");
        this.value = value;
    }

    public string Value => this.value;

    public static Result<TypeEvent, Error> CreateNew(string value)
    {
        if (value == null)
        {
            return Errors.General.ValueIsRequired();
        }
        if (!Enum.IsDefined(typeof(TypeEventEnum), value))
        {
            return new Error($"The value is a not a valid {typeof(TypeEventEnum)}", "TypeEventEnum.value");
        }

        return new TypeEvent(value);
    }

    public static implicit operator string(TypeEvent type)
    {
        return type.Value;
    }

    public static implicit operator TypeEvent(string value)
    {
        return new TypeEvent(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private enum TypeEventEnum
    {
        Poetry,
        Music,
        Photography
    }
}
