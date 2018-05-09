namespace Afterman.Notifications.Slack.Endpoint.Components
{
    using System.Threading.Tasks;
    using Contracts.Events;

    public interface ISendSlackNotifications
    {
        Task Send(IGotAnErrorMessage message);
    }
}
