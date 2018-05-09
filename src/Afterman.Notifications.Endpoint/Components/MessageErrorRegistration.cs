namespace Afterman.Notifications.Endpoint.Components
{
    using NServiceBus.Pipeline;

    public class MessageErrorRegistration :
        RegisterStep
    {
        public MessageErrorRegistration()
            : base(
                stepId: "MessageErrorBehavior",
                behavior: typeof(MessageErrorBehavior),
                description: "Reads off any and all Incoming Messages")
        {
        }
    }
}
