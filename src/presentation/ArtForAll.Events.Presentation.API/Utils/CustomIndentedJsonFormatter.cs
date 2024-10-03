using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace ArtForAll.Events.Presentation.API.Utils
{
    public class CustomIndentedJsonFormatter : ITextFormatter
    {
        private const int maxLineLength = 80;

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var logObject = new
            {
                Timestamp = logEvent.Timestamp,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Exception = logEvent.Exception?.ToString(),
                Properties = logEvent.Properties
            };

            var json = JsonConvert.SerializeObject(logObject, Formatting.Indented);
            output.WriteLine(json);
        }

        private string WrapMessage(string message, int? maxLineLength)
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            var wrappedMessage = new StringWriter();
            var lines = message.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                var words = line.Split(' ');
                int? currentLineLength = 0;

                foreach (var word in words)
                {
                    if (currentLineLength + word.Length + 1 > maxLineLength)
                    {
                        wrappedMessage.WriteLine();
                        currentLineLength = 0;
                    }

                    if (currentLineLength > 0)
                    {
                        wrappedMessage.Write(" ");
                        currentLineLength++;
                    }

                    wrappedMessage.Write(word);
                    currentLineLength += word.Length;
                }

                wrappedMessage.WriteLine(); // Maintain original newlines
            }

            return wrappedMessage.ToString().TrimEnd();
        }
    }
}
