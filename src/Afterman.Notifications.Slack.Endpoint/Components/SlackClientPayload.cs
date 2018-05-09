namespace Afterman.Notifications.Slack.Endpoint.Components
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SlackClientPayload
    {
        public SlackClientPayload()
        {
            this.Attachments = new List<SlackClientPayloadAttachment>();
        }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<SlackClientPayloadAttachment> Attachments { get; set; }

    }
}
