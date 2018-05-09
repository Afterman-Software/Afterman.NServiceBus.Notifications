
namespace Afterman.Notifications.Email.Endpoint
{
    using Components;
    using Contracts.Events;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            //TODO: change to real persistence of your choosing
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.SendFailedMessagesTo("error");

            endpointConfiguration.AuditProcessedMessagesTo("audit");

            endpointConfiguration.RegisterComponents(x =>
            {
                x.ConfigureComponent<ISendEmailNotifications>(
                    d => new DefaultEmailNotificationSender()
                    , DependencyLifecycle.InstancePerCall);
            });

            endpointConfiguration
                .Conventions()
                .DefiningCommandsAs(x => null != x.Namespace && x.Namespace.Contains("Contracts.Commands"))
                .DefiningEventsAs(x => null != x.Namespace && x.Namespace.Contains("Contracts.Events"))
                .DefiningMessagesAs(x => null != x.Namespace && x.Namespace.Contains("Contracts.Messages"));


            //TODO: pick your transport 
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(IGotAnErrorMessage), "Afterman.Notifications.Endpoint");

        }
    }
}
