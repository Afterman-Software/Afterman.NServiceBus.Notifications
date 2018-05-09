namespace Afterman.Notifications.Slack.Endpoint.Components
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Newtonsoft.Json;
    using NServiceBus.Logging;

    public class DefaultSlackNotificationSender : 
        ISendSlackNotifications
    {
        private static ILog Log = LogManager.GetLogger<DefaultSlackNotificationSender>();
        private static readonly string ProxyUser = ConfigurationManager.AppSettings["WebProxyUser"];
        private static readonly string ProxyPassword = ConfigurationManager.AppSettings["WebProxyPassword"];
        private static readonly string ProxyServer = ConfigurationManager.AppSettings["WebProxyServer"];
        private static readonly string ProxyDomain = ConfigurationManager.AppSettings["WebProxyDomain"];
        private static readonly string EnvironmentName = ConfigurationManager.AppSettings["EnvironmentName"];
        private static readonly string ServicePulseBaseUrl = ConfigurationManager.AppSettings["ServicePulseBaseUrl"];
        private readonly Uri SlackUri = new Uri(ConfigurationManager.AppSettings["SlackUri"]);

        private readonly Encoding _encoding = new UTF8Encoding();

        public async Task Send(IGotAnErrorMessage message)
        {
            await Send(GetMessage(message));
        }

        private SlackClientPayload GetMessage(IGotAnErrorMessage message)
        {
            return new SlackClientPayload()
            {
                Attachments = new List<SlackClientPayloadAttachment>
                {
                    new SlackClientPayloadAttachment
                    {
                        Color = "#ff0000",
                        ImageUrl = null,
                        Text = $"{message.ExceptionMessage}",
                        Title = $"Exception in {EnvironmentName}",
                        TitleLink = $"{ServicePulseBaseUrl}/#/failed-messages/message/{message.MessageId}",
                        Fields = new List<SlackClientPayloadAttachmentField>
                        {
                            new SlackClientPayloadAttachmentField
                            {
                                Title = "Environment",
                                Value = EnvironmentName,
                            },
                            new SlackClientPayloadAttachmentField
                            {
                                Title = "Endpoint",
                                Value = message.ProcessingEndpoint,
                            },
                            new SlackClientPayloadAttachmentField
                            {
                                Title = "Message Type",
                                Value = message.MessageType,
                            },
                            new SlackClientPayloadAttachmentField
                            {
                                Title = "Message ID",
                                Value = message.MessageId,
                            },
                            new SlackClientPayloadAttachmentField
                            {
                                Title = "Stack Trace",
                                Value = message.StackTrace,
                            },

                        }
                    }
                }
            };
        }


        private async Task Send(SlackClientPayload payload)
        {
            Log.Debug($"posting message to slack");
            string payloadJson = JsonConvert.SerializeObject(payload);
            using (var client = new WebClient())
            {
                if (!String.IsNullOrEmpty(ProxyServer))
                {
                    client.Proxy = new WebProxy
                    {
                        Address = new Uri(ProxyServer),
                        Credentials = new NetworkCredential(ProxyUser, ProxyPassword, ProxyDomain),
                    };
                }
                var data = new NameValueCollection {["payload"] = payloadJson};
                var response = await client.UploadValuesTaskAsync(SlackUri, "POST", data);
                var responseText = _encoding.GetString(response);
                Log.Debug($"Posted message to slack; response - {responseText}");
            }
        }
    }
}
