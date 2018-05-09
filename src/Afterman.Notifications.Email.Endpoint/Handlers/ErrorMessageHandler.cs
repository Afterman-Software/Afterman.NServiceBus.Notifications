namespace Afterman.Notifications.Email.Endpoint.Handlers
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
        private readonly ISendEmailNotifications _emailNotificationSender;

        public ErrorMessageHandler(ISendEmailNotifications emailNotificationSender)
        {
            this._emailNotificationSender = emailNotificationSender;
        }

        public async Task Handle(IGotAnErrorMessage message, IMessageHandlerContext context)
        {
            Log.Debug($"Sending email notification for failed message {message.MessageId}");
            await this._emailNotificationSender.Send(message);
            Log.Debug($"Email notification sent for failed message {message.MessageId}");
        }
    }
}
