namespace Afterman.Notifications.Email.Endpoint.Components
{
    using System;
    using System.Configuration;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.Events;
    using NServiceBus.Logging;

    public class DefaultEmailNotificationSender :
        ISendEmailNotifications
    {
        private static ILog Log = LogManager.GetLogger<DefaultEmailNotificationSender>();
        private static readonly string EnvironmentName = ConfigurationManager.AppSettings["EnvironmentName"];
        private static readonly string ErrorDistributionGroupEmail = ConfigurationManager.AppSettings["ErrorDistributionGroupEmail"];
        private static readonly string ServicePulseBaseUrl = ConfigurationManager.AppSettings["ServicePulseBaseUrl"];
        private static readonly string SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        private static readonly int SmtpPort = Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        public async Task Send(IGotAnErrorMessage message)
        {
            await Send(GetMessage(message));
        }

        private MailMessage GetMessage(IGotAnErrorMessage message)
        {
            var email = new MailMessage
            {
                Subject = $"Exception in {EnvironmentName}",
                IsBodyHtml = true,
                From = new MailAddress("errors@noreply.com"),
                Body = $"<a href='{ServicePulseBaseUrl}/#/failed-messages/message/{message.MessageId}'>{message.ExceptionMessage}</a>" +
                    "\r\n" +
                    message.StackTrace,
            };
            email.To.Add(ErrorDistributionGroupEmail);
            return email;
        }

        private async Task Send(MailMessage message)
        {
            Log.Debug($"sending email");
            using (var server = new SmtpClient(SmtpServer, SmtpPort))
            {
                await server.SendMailAsync(message);
            }
            Log.Debug($"sent email");
        }
    }
}
