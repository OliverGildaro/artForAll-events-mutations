namespace ArtForAll.Events.UnitTests.TestData
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;

    public class CreateEventCommandHandlerTestData : TheoryData<CreateEventCommand>
    {
        public CreateEventCommandHandlerTestData()
        {
            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                date = DateTime.UtcNow.AddDays(1),
                Type = "Music",
               
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for The Doors",
                Description = "Description2",
                date = DateTime.UtcNow.AddDays(100),
                Type = "Poetry"
            });

            this.Add(new CreateEventCommand()
            {
                Name = null,
                Description = "Description",
                date = DateTime.UtcNow.AddDays(1),
                Type = "Music"
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = null,
                date = DateTime.UtcNow.AddDays(1),
                Type = "Music"
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                date = DateTime.UtcNow.AddDays(1),
                Type = "Music"
            });

            this.Add(new CreateEventCommand()
            {
                Name = "Concert for Pink Floyd",
                Description = "Description",
                date = DateTime.UtcNow.AddDays(1),
                Type = null
            });

            this.Add(new CreateEventCommand()
            {
                Name = null,
                Description = null,
                date = DateTime.UtcNow.AddDays(1),
                Type = null
            });
        }
    }
}
