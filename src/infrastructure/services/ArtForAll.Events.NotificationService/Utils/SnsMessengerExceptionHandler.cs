namespace ArtForAll.Events.NotificationService.Utils
{
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using ArtForAll.Events.NotificationService.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Extensions.Logging;

    public class SnsMessengerExceptionHandler : ExceptionHandlerBase, ISnsMessengerExceptionHandler
    {
        public SnsMessengerExceptionHandler(ILogger<SnsMessengerExceptionHandler> logger)
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

            this.Catch<AmazonSimpleNotificationServiceException>(ex =>
            {
                logger.LogError($"AmazonSimpleNotificationServiceException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<TimeoutException>(ex =>
            {
                logger.LogError($"TimeoutException: {ex.Message}");
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
