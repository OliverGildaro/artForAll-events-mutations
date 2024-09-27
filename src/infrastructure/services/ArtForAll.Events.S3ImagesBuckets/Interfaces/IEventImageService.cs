namespace ArtForAll.Events.S3ImagesBuckets.Interfaces
{
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Shared.ErrorHandler;

    public interface IEventImageService
    {
        Task<Result> UploadImageAsync(ImageBuffer imageFile);
        Task<Result> UpdateImageAsync(ImageBuffer image);
    }
}
