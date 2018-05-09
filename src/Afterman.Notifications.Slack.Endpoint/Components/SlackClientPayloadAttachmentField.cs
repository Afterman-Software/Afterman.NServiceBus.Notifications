namespace Afterman.Notifications.Slack.Endpoint.Components
{
    using Newtonsoft.Json;

    public class SlackClientPayloadAttachmentField
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool Short { get; set; }
    }
}
