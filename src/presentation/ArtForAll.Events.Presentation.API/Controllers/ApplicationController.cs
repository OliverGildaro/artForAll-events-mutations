namespace ArtForAll.Presentation.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class ApplicationController : ControllerBase
    {
        protected static byte[] ConvertIFormFileToByteArrayAsync(IFormFile file)
        {
            if (file is null)
            {
                return null;
            }
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
