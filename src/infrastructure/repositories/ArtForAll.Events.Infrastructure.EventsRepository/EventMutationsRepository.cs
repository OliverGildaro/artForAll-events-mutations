namespace ArtForAll.Events.Infrastructure.EFRepository
{
    using System.Threading.Tasks;
    using Amazon.Runtime.Internal;
    using ArtForAll.Events.Core.DomainModel.Entities;
    using ArtForAll.Events.Infrastructure.EFRepository.Interfaces;
    using ArtForAll.Infrastructure.EFRepositories.Interfaces;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
    using ArtForAll.Shared.ExceptionHandler;
    using Microsoft.EntityFrameworkCore;
    using OCP.PortalEvents.Repositories.Context;

    public class EventMutationsRepository : IEventMutationsRepository
    {
        private readonly EventsContext context;
        private readonly IEventRepositoryExceptionHandler exceptionHandler;

        public EventMutationsRepository(EventsContext context, IEventRepositoryExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.exceptionHandler = exceptionHandler;
        }

        public async Task<Result<Event, Error>> FindAsync(string entityId)
        {
            var entity = await this.context.Events.Where(x => x.Id == entityId)
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                return Result<Event, Error>.Failure(new Error("Error", "404")); ;
            }

            return Result<Event, Error>.Success(entity);
        }

        public Result Insert(Event entity)
        {
            if (entity is null)
            {
                return Result.Failure("");
            }

            try
            {
                context.Set<Event>().Add(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            return Result.Success();
        }

        public async Task<Result> SaveChangesAsync()
        {
            return await this.exceptionHandler.HandleAsync(async () => await context.SaveChangesAsync());
        }
    }
}
