namespace Afterman.Notifications.Slack.Endpoint.Components
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SlackClientPayloadAttachment
    {
        public SlackClientPayloadAttachment()
        {
            this.Fields = new List<SlackClientPayloadAttachmentField>();
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("pretext")]
        public string Pretext { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_link")]
        public string TitleLink { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("fields")]
        public IList<SlackClientPayloadAttachmentField> Fields { get; set; }



    }
}
