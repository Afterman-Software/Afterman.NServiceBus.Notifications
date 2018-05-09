namespace Afterman.Notifications.Contracts.Events
{
    public interface IGotAnErrorMessage
    {
        string MessageId { get; set; }
        string MessageType { get; set; }
        string OriginatingEndpoint { get; set; }
        string ProcessingEndpoint { get; set; }
        string ExceptionMessage { get; set; }
        string StackTrace { get; set; }
    }
}
