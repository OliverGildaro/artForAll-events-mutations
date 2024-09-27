namespace ArtForAll.Events.UnitTests.TestData
{
    using ArtForAll.Events.Core.Commanding.Events.PatchEvent;
    using ArtForAll.Events.Presentation.DTOs.Events;
    using Microsoft.AspNetCore.JsonPatch;
    using Newtonsoft.Json;

    public class PatchEventCommandHandlerTestData : TheoryData<PatchEventCommand>
    {
        private const string jsonsReplace = "[{\"op\": \"replace\",\"path\": \"/name\",\"value\":\"Poetry circus\"},{\"op\": \"replace\",\"path\": \"/description\",\"value\":\"Poetry circus description\"}]";
        private const string jsonMove = "[{\"op\": \"move\",\"from\": \"/description\",\"path\": \"/name\"}]";
        private const string jsonsRemove = "[{\"op\": \"remove\",\"path\": \"/description\"}]";
        private const string jsonCopy = "[{\"op\": \"copy\",\"from\": \"/name\",\"path\": \"/description\"}]";
        private const string jsonCopyFails = "[{\"op\": \"copy\",\"from\": \"/name\",\"path\": \"/description\"}]";

        public PatchEventCommandHandlerTestData()
        {
            this.Add(new PatchEventCommand
            {
                EventId = "123qwe",
                PatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonsReplace)
            });

            this.Add(new PatchEventCommand
            {
                EventId = "123qwe",
                PatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonMove)
            });

            this.Add(new PatchEventCommand
            {
                EventId = "123qwe",
                PatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonsRemove)
            });

            this.Add(new PatchEventCommand
            {
                EventId = "123qwe",
                PatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonCopy)
            });

            this.Add(new PatchEventCommand
            {
                EventId = "",
                PatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<EventPatchRequest>>(jsonCopyFails)
            });
        }
    }
}
