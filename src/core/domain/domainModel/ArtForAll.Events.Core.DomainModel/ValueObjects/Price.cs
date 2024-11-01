
namespace ArtForAll.Events.Core.DomainModel.ValueObjects;

using ArtForAll.Shared.Contracts.DDD;
using ArtForAll.Shared.ErrorHandler;


public class Price : ValueObject
{
    public static Price NonePrice { get; private set; } = new Price(null, null);
    public string? CurrencyExchange { get; private set; }
    public float? MonetaryValue { get; private set; }

    protected Price() {}

    private Price(string? currencyExchange, float? monetaryValue)
    {
        this.CurrencyExchange = currencyExchange;
        this.MonetaryValue = monetaryValue;
    }

    public static Result<Price, Error> CreateNew(string currencyExchange, float? monetaryValue)
    {
        if (string.IsNullOrEmpty(currencyExchange))
        {
            return Result<Price, Error>.Failure(new Error("", ""));
        }
        if (monetaryValue < 0)
        {
        }
        //value.Value asi consigo sacar el valor del objecto que se retorna
        //return new Price(currencyExchange, monetaryValue);
        return Result<Price, Error>.Success(new Price(currencyExchange, monetaryValue));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public static Price operator *(Price price, uint multiplier)
    {
        var result = new Price(
            price.CurrencyExchange, price.MonetaryValue * multiplier);
        return result;
    }
}
