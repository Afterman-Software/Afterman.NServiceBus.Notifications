
namespace Afterman.Notifications.Endpoint
{
    using Contracts.Events;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            //TODO: change to real persistence of your choosing
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            //TODO: disable errors, or forward them to another queue.  Moving them to "error" could end up sending them back here, 
            //depending on your system configuration
            endpointConfiguration.SendFailedMessagesTo("error");

            //TODO: 
            //  you may either point serviceControl error forwarding to Afterman.Notifications.Endpoint
            //  *OR*
            //  you may change the below line to use the queue that your system is currently forwarding error messages to
            //endpointConfiguration.DefineEndpointName("error.log");

            endpointConfiguration.AuditProcessedMessagesTo("audit");

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
