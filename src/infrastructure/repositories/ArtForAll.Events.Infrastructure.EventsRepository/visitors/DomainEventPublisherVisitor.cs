namespace ArtForAll.Events.Infrastructure.EFRepository.visitors
{
    using ArtForAll.Events.Core.DomainModel.Events;
    using ArtForAll.Events.Core.DomainModel.Interfaces;
    using ArtForAll.Events.NotificationService;
    using ArtForAll.Events.NotificationService.Interfaces;

    public class DomainEventPublisherVisitor : IDomainEventVisitor
    {
        private readonly ISnsMessengerService snsMessenger;
        private readonly ISnsMessengerExceptionHandler exceptionHandler;

        public DomainEventPublisherVisitor(ISnsMessengerService snsMessenger, ISnsMessengerExceptionHandler exceptionHandler)
        {
            this.snsMessenger = snsMessenger;
            this.exceptionHandler = exceptionHandler;
        }
        public async Task Visit(EventCreated eventCreated)
        {
            await this.exceptionHandler.HandleAsync(async () => await this.snsMessenger.PublishAsync(eventCreated));
        }

        public async Task Visit(EventPatched eventPatched)
        {
            await this.exceptionHandler.HandleAsync(async () => await this.snsMessenger.PublishAsync(eventPatched));
        }

        public async Task Visit(EventPublished concretePublished)
        {
            await this.exceptionHandler.HandleAsync(async () => await this.snsMessenger.PublishAsync(concretePublished));
        }

        public async Task Visit(EventDeleted eventDeleted)
        {
            await this.exceptionHandler.HandleAsync(async () => await this.snsMessenger.PublishAsync(eventDeleted));
        }

        public async Task Visit(ImageAdded imagePublished)
        {
            await this.exceptionHandler.HandleAsync(async () => await this.snsMessenger.PublishAsync(imagePublished));
        }
    }
}
