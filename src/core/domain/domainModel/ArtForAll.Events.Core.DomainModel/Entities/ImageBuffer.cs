namespace ArtForAll.Events.Core.DomainModel.Entities
{
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;

    public class ImageBuffer : Entity
    {
        private byte[] content;
        private string key;

        private ImageBuffer(byte[] content, string imageId)
        {
            content = content ?? throw new ArgumentNullException(nameof(content), "Parameter cannot be null");
            this.Id = imageId;
            this.content = content;
            this.key = $"images/{imageId}";
        }

        public string Key => key;
        public byte[] Content => content;
        public static Result<ImageBuffer, Error> CreateNew(byte[] content, string imageId)
        {
            return new ImageBuffer(content, imageId);
        }
    }
}
