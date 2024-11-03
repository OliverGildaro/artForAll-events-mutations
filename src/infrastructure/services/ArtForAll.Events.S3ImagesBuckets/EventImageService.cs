namespace ArtForAll.Events.S3ImagesBuckets
{
    using Amazon.S3.Model;
    using Amazon.S3;
    using System.Net;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Shared.ErrorHandler;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;

    public class EventImageService : IEventImageService
    {
        private readonly IAmazonS3 s3;
        private readonly IS3BucketsExceptionHandler exceptionHandler;
        private readonly string bucketName = "artforallbucket";

        //public EventImageService(IAmazonS3 s3, IS3BucketsExceptionHandler exceptionHandler)
        //{
        //    this.s3 = s3;
        //    this.exceptionHandler = exceptionHandler;
        //}

        public async Task<Result> UploadImageAsync(ImageBuffer image)
        {
            using var stream = new MemoryStream(image.Content);
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = image.Key,
                InputStream = stream,
            };

            //var response = await this.exceptionHandler.HandleAsync(async () => await s3.PutObjectAsync(putObjectRequest));

            //if (response.HttpStatusCode == HttpStatusCode.OK)
            //{
            //    return Result.Success(image.Id);
            //}
            return Result.Success("Sucess");
        }

        public async Task<Result> UpdateImageAsync(ImageBuffer image)
        {
            return await this.exceptionHandler.HandleAsync(async () => await UploadImageAsync(image));
        }
    }
}
