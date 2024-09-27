namespace ArtForAll.Events.Presentation.API.ExceptionHandlers
{
    using ArtForAll.Events.Presentation.API.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Extensions.Logging;

    public class EventMutationsExceptionHandler : ExceptionHandlerBase, IEventMutationsExceptionHandler
    {
        public EventMutationsExceptionHandler(ILogger<EventMutationsExceptionHandler> logger)
        {
            Catch<ArgumentNullException>(ex =>
            {
                logger.LogError($"ArgumentNullException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<ArgumentException>(ex =>
            {
                logger.LogError($"ArgumentException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<InvalidOperationException>(ex =>
            {
                logger.LogError($"InvalidOperationException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<TimeoutException>(ex =>
            {
                logger.LogError($"TimeoutException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<HttpRequestException>(ex =>
            {
                logger.LogError($"HttpRequestException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<UnauthorizedAccessException>(ex =>
            {
                logger.LogError($"UnauthorizedAccessException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<NotImplementedException>(ex =>
            {
                logger.LogError($"NotImplementedException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<NullReferenceException>(ex =>
            {
                logger.LogError($"NullReferenceException: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });

            Catch<Exception>(ex =>
            {
                logger.LogError($"Exception: {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
            });
        }
    }
}
