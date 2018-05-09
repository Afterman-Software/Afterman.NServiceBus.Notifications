namespace Afterman.Notifications.Email.Endpoint.Components
{
    using System.Threading.Tasks;
    using Contracts.Events;

    public interface ISendEmailNotifications
    {
        Task Send(IGotAnErrorMessage message);
    }
}
