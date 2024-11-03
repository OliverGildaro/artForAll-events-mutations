using ArtForAll.Events.Core.Commanding.AgendaItems;
using ArtForAll.Events.Presentation.API.Interfaces;
using ArtForAll.Events.Presentation.DTOs.AgendaItems;
using ArtForAll.Presentation.API.Controllers;
using ArtForAll.Presentation.API.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ArtForAll.Events.Presentation.API.Controllers
{
    [Route("api/events")]
    [EnableCors("AllowSpecificOrigins")]
    [Produces("application/json")]
    public class AgendaItemsController : ApplicationController
    {
        private readonly CommandDispatcher messages;
        private readonly IEventMutationsExceptionHandler exceptionHandler;

        public AgendaItemsController(CommandDispatcher messages, IEventMutationsExceptionHandler exceptionHandler)
        {
            this.messages = messages;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpPost("{eventId}/agendaItems")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAgendaItem(string eventId, [FromBody] AgendaItemCreateRequest request)
        {
            var command = new AddAgendaItemCommand
            {
                EventId = eventId,
                Name = request.Name,
                Description = request.Description,
                ScheduleDate = request.ScheduleDate,
                Duration = request.Duration,
            };

            try
            {
                var agendaItemAddedResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(command));

                if (agendaItemAddedResult.IsFailure)
                {
                    return BadRequest();
                }

                return StatusCode(StatusCodes.Status201Created, new { id = agendaItemAddedResult.Id });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
