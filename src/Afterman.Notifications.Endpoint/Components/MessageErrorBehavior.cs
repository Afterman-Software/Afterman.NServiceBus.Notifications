namespace Afterman.Notifications.Endpoint.Components
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Extensions;
    using NServiceBus;
    using NServiceBus.Logging;
    using NServiceBus.Pipeline;

    public class MessageErrorBehavior :
        Behavior<IIncomingPhysicalMessageContext>
    {
        private static readonly ILog Log = LogManager.GetLogger<MessageErrorBehavior>();

        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            //only if error message
            var exceptionMessage = context.SafeGetMessageHeader("NServiceBus.ExceptionInfo.Message");
            if (string.IsNullOrEmpty(exceptionMessage))
            {
                await next();
                return;
            }
            Log.Debug($"Message failed '{context.SafeGetMessageHeader(Headers.MessageId)}', " +
                        $"type: {context.SafeGetMessageHeader(Headers.EnclosedMessageTypes)}");

            await context.Publish<IGotAnErrorMessage>(x =>
            {
                x.MessageId = context.SafeGetMessageHeader(Headers.MessageId);
                x.MessageType = context.SafeGetMessageHeader(Headers.EnclosedMessageTypes);
                x.OriginatingEndpoint = context.SafeGetMessageHeader(Headers.OriginatingEndpoint);
                x.ProcessingEndpoint = context.SafeGetMessageHeader(Headers.ProcessingEndpoint);
                x.ExceptionMessage = exceptionMessage;
                x.StackTrace = context.SafeGetMessageHeader("NServiceBus.ExceptionInfo.StackTrace");
            });
        }
    }
}
