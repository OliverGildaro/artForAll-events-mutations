namespace ArtForAll.Events.Infrastructure.EFRepository.utils
{
    using ArtForAll.Events.Infrastructure.EFRepository.Interfaces;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class EventRepositoryExceptionHandler : ExceptionHandlerBase, IEventRepositoryExceptionHandler
    {
        public EventRepositoryExceptionHandler(ILogger<EventRepositoryExceptionHandler> logger)
        {
            this.Catch<ArgumentNullException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            this.Catch<Exception>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<DbUpdateConcurrencyException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<DbUpdateException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<SqlException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<TimeoutException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<InvalidOperationException>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });

            Catch<Exception>(ex =>
            {
                logger.LogError("Exception message" + ex.Message);
                logger.LogError("Exception stacktrace" + ex.StackTrace);
            });
        }
    }
}
