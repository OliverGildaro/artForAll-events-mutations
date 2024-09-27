namespace ArtForAll.Events.Presentation.DTOs.Images
{
    using Microsoft.AspNetCore.Http;

    public record ImageAddRequest(
        string eventId,
        IFormFile? Image);
}
