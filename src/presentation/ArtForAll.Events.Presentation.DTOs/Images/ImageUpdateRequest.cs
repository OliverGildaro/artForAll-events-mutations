using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtForAll.Events.Presentation.DTOs.Images
{
    public class ImageUpdateRequest
    {
        [FromForm]
        public string EventId { get; set; }
        [FromForm]
        public IFormFile Image { get; set; }
    }
}
