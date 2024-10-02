namespace ArtForAll.Events.UnitTests.TestData
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Core.DomainModel.ValueObjects;
    using ArtForAll.Events.Presentation.DTOs.Events;

    public class CreateEventCommandHandlerTestData : TheoryData<CreateEventCommand>
    {
        public CreateEventCommandHandlerTestData()
        {
            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = "Music",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country ="Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for The Doors",
                Description = "Description2",
                StartDate = DateTime.UtcNow.AddDays(100),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = "Poetry",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = null,
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = "Music",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = null,
                StartDate = DateTime.UtcNow.AddDays(1),
                Type = "Music",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = "Music",
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = null,
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });

            this.Add(new CreateEventCommand()
            {
                Name = null,
                Description = null,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Type = null,
                Address = new AddressRequest
                {
                    City = "Cochabamba",
                    Country = "Bolivia",
                    Number = "088",
                    Street = "Bartolome c",
                    ZipCode = "0000"
                },
                Capacity = 100,
                Price = new PriceRequest
                {
                    CurrencyExchange = "$",
                    MonetaryValue = 3.55f
                },
            });
        }
    }
}
