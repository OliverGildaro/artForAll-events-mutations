using ArtForAll.Core.Commanding.Events.CreateEvent;
using ArtForAll.Events.Presentation.API.Interfaces;
using ArtForAll.Events.Presentation.DTOs.Images;
using ArtForAll.Presentation.API.Utils;
using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ArtForAll.Presentation.API.Controllers
{
    [Route("api/images")]
    [EnableCors("AllowSpecificOrigins")]
    public class ImagesController : ApplicationController
    {
        private readonly CommandDispatcher messages;
        private readonly IImageMutationsExceptionHandler exceptionHandler;

        public ImagesController(CommandDispatcher messages, IImageMutationsExceptionHandler exceptionHandler)
        {
            this.messages = messages;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddImage([FromForm] ImageAddRequest request)
        {
            try
            {

                var addImageCmd = new AddImageCommand
                {
                    Id = request.eventId,
                    ImageContent = ConvertIFormFileToByteArrayAsync(request.Image),
                    ContentType = request.Image.ContentType,
                    FileName = request.Image.FileName
                };

                var imageUploadResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(addImageCmd));

                if (imageUploadResult.IsFailure)
                {
                    return BadRequest();
                }

                return StatusCode(StatusCodes.Status201Created, new { id = imageUploadResult.Id });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]//This is for a CRUD based API
        [Consumes("multipart/form-data")]
        [ProducesResponseType(203)]
        public async Task<IActionResult> UpdateImage([FromRoute] string id, [FromForm] ImageUpdateRequest request)
        {
            try
            {
                var updateImageCmd = new UpdateImageCommand
                {
                    Id = id,
                    EventId = request.EventId,
                    ImageContent = ConvertIFormFileToByteArrayAsync(request.Image),
                    ContentType = request.Image.ContentType,
                    FileName = request.Image.FileName
                };

                var imageUpdatedResult = await this.exceptionHandler.HandleAsync(() => this.messages.Dispatch(updateImageCmd));

                if (imageUpdatedResult.IsFailure)
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
