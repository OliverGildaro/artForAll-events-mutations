namespace ArtForAll.Events.Core.DomainModel.Entities
{
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public class Image : Entity
    {
        private string eventId;
        private string contentType;
        private string fileName;

        protected Image()
        {
            
        }

        private Image(string eventId, string contentType, string fileName)
        {
            this.Id = Guid.NewGuid().ToString();
            this.eventId = eventId;
            this.contentType = contentType;
            this.fileName = fileName;
        }

        public string ContentType => contentType;
        public string FileName => fileName;
        public string EventId => eventId;
        public static Result<Image, Error> CreateNew(string eventId, string contentType, string fileName)
        {
            return new Image(eventId, contentType, fileName);
        }

        internal void Update(string contentType, string fileName)
        {
            this.contentType = contentType;
            this.fileName = fileName;   
        }
    }
}
