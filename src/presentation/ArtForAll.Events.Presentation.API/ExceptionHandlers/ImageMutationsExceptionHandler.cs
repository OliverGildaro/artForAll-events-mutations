namespace ArtForAll.Events.Presentation.API.Utils
{
    using ArtForAll.Events.Presentation.API.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Extensions.Logging;

    public class ImageMutationsExceptionHandler : ExceptionHandlerBase, IImageMutationsExceptionHandler
    {
        public ImageMutationsExceptionHandler(ILogger<ImageMutationsExceptionHandler> logger)
        {
            this.Catch<ArgumentNullException>(ex =>
            {
                logger.LogError($"ArgumentNullException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<ArgumentException>(ex =>
            {
                logger.LogError($"ArgumentException: {ex.Message}");
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

            this.Catch<HttpRequestException>(ex =>
            {
                logger.LogError($"HttpRequestException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<UnauthorizedAccessException>(ex =>
            {
                logger.LogError($"UnauthorizedAccessException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<NotImplementedException>(ex =>
            {
                logger.LogError($"NotImplementedException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            this.Catch<NullReferenceException>(ex =>
            {
                logger.LogError($"NullReferenceException: {ex.Message}");
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
