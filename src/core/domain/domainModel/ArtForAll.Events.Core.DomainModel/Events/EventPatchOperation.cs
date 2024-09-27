namespace ArtForAll.Events.Core.Commanding.Events.PatchEvent
{
    public class EventPatchOperation
    {
        public string Path { get; set; }
        public string Op { get; set; }
        public object Value { get; set; }
    }
}
