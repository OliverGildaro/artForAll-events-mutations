namespace ArtForAll.Events.S3ImagesBuckets.Utils
{
    using System;
    using Amazon.Runtime;
    using Amazon.Runtime.Internal;
    using Amazon.S3;
    using ArtForAll.Events.S3ImagesBuckets.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Extensions.Logging;

    public class S3BucketsExceptionHandler : ExceptionHandlerBase, IS3BucketsExceptionHandler
    {
        public S3BucketsExceptionHandler(ILogger<S3BucketsExceptionHandler> logger)
        {
            this.Catch<AmazonServiceException>(ex =>
            {
                logger.LogError($"AmazonServiceException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<AmazonClientException>(ex =>
            {
                logger.LogError($"AmazonClientException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<InvalidOperationException>(ex =>
            {
                logger.LogError($"InvalidOperationException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<TimeoutException>(ex =>
            {
                logger.LogError($"TimeoutException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<HttpErrorResponseException>(ex =>
            {
                logger.LogError($"Exception: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });
            this.Catch<AmazonS3Exception>(ex =>
            {
                logger.LogError($"Exception: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });
            
            this.Catch<Exception>(ex =>
            {
                logger.LogError($"Exception: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });
        }
    }
}
