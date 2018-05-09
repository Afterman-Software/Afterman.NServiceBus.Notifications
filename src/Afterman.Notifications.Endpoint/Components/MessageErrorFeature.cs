namespace Afterman.Notifications.Endpoint.Components
{
    using NServiceBus.Features;

    public class MessageErrorFeature :
        Feature
    {
        public MessageErrorFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var pipeline = context.Pipeline;
            pipeline.Register<MessageErrorRegistration>();
        }
    }
}
