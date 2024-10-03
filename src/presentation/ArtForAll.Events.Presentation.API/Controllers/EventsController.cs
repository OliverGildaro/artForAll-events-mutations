namespace ArtForAll.Presentation.API.Controllers
{
    using ArtForAll.Core.Commanding.Events.CreateEvent;
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.Core.Commanding.Events.PublishEvent;
    using ArtForAll.Events.Presentation.API.Interfaces;
    using ArtForAll.Events.Presentation.DTOs.Events;
    using ArtForAll.Presentation.API.Utils;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/events")]
    [EnableCors("AllowSpecificOrigins")]
    [Produces("application/json")]
    public class EventsController : ApplicationController
    {
        private readonly CommandDispatcher messages;
        private readonly IEventMutationsExceptionHandler exceptionHandler;

        public EventsController(CommandDispatcher messages, IEventMutationsExceptionHandler exceptionHandler)
        {
            this.messages = messages;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequest request)
        {
            var command = new CreateEventCommand
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Type = request.Type,
                Price = request.Price,
                Address = request.Address,
                Capacity = request.Capacity,
            };

            try
            {
                var eventCreatedResult = await this.exceptionHandler.HandleAsync(() =>this.messages.Dispatch(command));

                if (eventCreatedResult.IsFailure)
                {
                    return BadRequest();
                }

                return StatusCode(StatusCodes.Status201Created, new { id = eventCreatedResult.Id });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPatch("{id}")]//This is for a CRUD based API
        [ProducesResponseType(203)]
        public async Task<IActionResult> PartiallUpdateEvent(string id, JsonPatchDocument<EventPatchRequest> patchDocument)
        {
            try
            {
                //var patchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonPatch);
                var patchResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(new PatchEventCommand
                {
                    EventId = id,
                    PatchDocument = patchDocument,
                }));

                if (patchResult.IsFailure)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}/publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PublishEvent(string id)
        {
            try
            {
                var patchResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(new PublishEventCommand
                {
                    Id = id,
                }));

                if (patchResult.IsFailure)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            try
            {
                var patchResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(new DeleteEventCommand
                {
                    Id = id,
                }));

                if (patchResult.IsFailure)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
