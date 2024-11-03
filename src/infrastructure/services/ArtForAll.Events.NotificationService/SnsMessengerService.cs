namespace ArtForAll.Events.NotificationService
{
    using Amazon.SimpleNotificationService;
    using Amazon.SimpleNotificationService.Model;
    using System.Text.Json;

    public class SnsMessengerService : ISnsMessengerService
    {
        private readonly IAmazonSimpleNotificationService sns;
        private string? topicArn;

        //public SnsMessengerService(IAmazonSimpleNotificationService sns)
        //{
        //    this.sns = sns;
        //}

        public async Task<PublishResponse> PublishAsync<T>(T message)
        {
            string _TopicArn = await GetTopicArnAsync();

            var sendMessageRequest = new PublishRequest
            {
                TopicArn = _TopicArn,
                Message = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    }
                },
            };
            return await sns.PublishAsync(sendMessageRequest);
        }

        private async Task<string> GetTopicArnAsync()
        {
            if (topicArn is not null)
            {
                return topicArn;
            }

            var queueUrlRes = await sns.FindTopicAsync("events");
            topicArn = queueUrlRes.TopicArn;
            return topicArn;
        }
    }
}
