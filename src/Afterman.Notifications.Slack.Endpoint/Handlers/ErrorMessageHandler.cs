namespace Afterman.Notifications.Slack.Endpoint.Handlers
{
    using System;
    using System.Threading.Tasks;
    using Components;
    using Contracts.Events;
    using NServiceBus;
    using NServiceBus.Logging;

    public class ErrorMessageHandler :
        IHandleMessages<IGotAnErrorMessage>
    {

        private static ILog Log = LogManager.GetLogger<ErrorMessageHandler>();
        private readonly ISendSlackNotifications _slackNotificationSender;

        public ErrorMessageHandler(ISendSlackNotifications slackNotificationSender)
        {
            this._slackNotificationSender = slackNotificationSender;
        }

        public async Task Handle(IGotAnErrorMessage message, IMessageHandlerContext context)
        {
            Log.Debug($"Sending slack notification for failed message {message.MessageId}");
            await this._slackNotificationSender.Send(message);
            Log.Debug($"Slack notification send for failed message {message.MessageId}");

        }
    }
}
