using System.Net;
using System.Net.Http.Headers;
using ArtForAll.Events.IntegrationTests.Models;
using ArtForAll.Events.Presentation.DTOs.Events;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ArtForAll.Events.IntegrationTests.EventsEndpoints
{
    public class EventControllerTests : IClassFixture<WebApplicationFactory<Presentation.API.Utils.Program>>
    {
        private readonly HttpClient httpClient;
        public EventControllerTests(WebApplicationFactory<Presentation.API.Utils.Program> factory)
        {
            this.httpClient = factory.CreateDefaultClient();
        }

        //[Fact]
        //public async Task asasas()
        //{
        //    var request = GetValidEventModel().CloneWith(m => m.Name = null);
        //    var content = GetMultipartFormDataContent(request);
        //    var response = await this.httpClient.PostAsJsonAsync("/api/events", request);
        //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        //}

        private static TestEventInputModel GetValidEventModel()
        {
            return new TestEventInputModel
            {
                Name = "NewName",
                Description = "Description",
                Date = DateTime.UtcNow.AddDays(1),
                type = "Music",
            };
        }

        private MultipartFormDataContent GetMultipartFormDataContent(TestEventInputModel request)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(request.Name), nameof(TestEventInputModel.Name) },
                { new StringContent(request.Description ?? string.Empty), nameof(TestEventInputModel.Description) },
                { new StringContent(request.Date.ToString("o")), nameof(TestEventInputModel.Date) }, // "o" for ISO 8601 format
                { new StringContent(request.type), nameof(TestEventInputModel.type) }
            };
            return content;
        }
    }
}
