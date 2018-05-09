namespace Afterman.Notifications.Endpoint.Extensions
{
    using NServiceBus;

    public static class MessageContextExtensions
    {
        public static string SafeGetMessageHeader(this IMessageProcessingContext context, string headerName)
        {
            return context.MessageHeaders.ContainsKey(headerName) ? 
                context.MessageHeaders[headerName] : 
                null;
        }
    }
}
