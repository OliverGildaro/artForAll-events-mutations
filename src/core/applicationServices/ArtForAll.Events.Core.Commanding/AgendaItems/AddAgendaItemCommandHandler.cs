using ArtForAll.Events.Core.Commanding.decorators.Auditing;
using ArtForAll.Events.Core.DomainModel.Entities;
using ArtForAll.Infrastructure.EFRepositories.Interfaces;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Core.Commanding.AgendaItems;

[AuditLog]
public class AddAgendaItemCommandHandler : ICommandHandler<AddAgendaItemCommand, Result>
{
    private readonly IEventMutationsRepository repository;

    public AddAgendaItemCommandHandler(IEventMutationsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Result> HandleAsync(AddAgendaItemCommand command)
    {
        var eventResult = await this.repository.FindAsync(command.EventId);

        if (eventResult.IsFailure)
        {
            
        }

        var @event = eventResult.Value;

        var agendaResut = AgendaItem.CreateNew(command.Name, command.Description, command.ScheduleDate, command.Duration);
        if (agendaResut.IsFailure)
        {

        }

        var agendaItem = agendaResut.Value;


        var addedResult = @event.AddAgendaItem(agendaItem);

        if (addedResult.IsFailure)
        {

        }

        var result = await this.repository.SaveChangesAsync();
        return Result.Success(result.Id);
    }
}
