namespace ArtForAll.Events.Core.DomainModel.ValueObjects;

using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;

public class Address : ValueObject
{
    public static Address NoneAddress { get; private set; } = new Address("", "", "", "", "");
    public string City { get; private set; }
    public string Country { get; private set; }
    public string Number { get; private set; }
    public string Street { get; private set; }
    public string ZipCode { get; private set; }

    protected Address() {}

    private Address(string city, string country, string streetName, string Number, string zipCode)
    {
        this.City = city;
        this.Country = country;
        this.Number = Number;
        this.Street = streetName;
        this.ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new System.NotImplementedException();
    }

    public static Result<Address, Error> CreateNew(string city, string country, string streetName, string Number, string zipCode)
    {
        if (string.IsNullOrEmpty(city))
        {
            //Result
        }
        return new Address(city, country, streetName, Number, zipCode);
    }
}
