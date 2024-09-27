namespace ArtForAll.Events.IntegrationTests.Models
{
    public class TestEventInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string type { get; set; }
        public IFormFile Image { get; set; }

        public TestEventInputModel CloneWith(Action<TestEventInputModel> changes)
        {
            var clone = (TestEventInputModel)MemberwiseClone();

            changes(clone);

            return clone;
        }
    }
}
