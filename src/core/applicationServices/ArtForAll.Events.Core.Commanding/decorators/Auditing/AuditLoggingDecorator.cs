namespace ArtForAll.Events.Core.Commanding.decorators.Auditing
{
    using ArtForAll.Shared.Contracts.CQRS;

    public sealed class AuditLoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand, TResult> handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand, TResult> handler)
        {
            this.handler = handler;
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            Console.WriteLine($"Handling command of type {command.GetType().Name}");
            var result = await handler.HandleAsync(command);
            Console.WriteLine($"Handled command of type {command.GetType().Name}");
            return result;
        }
    }
}
