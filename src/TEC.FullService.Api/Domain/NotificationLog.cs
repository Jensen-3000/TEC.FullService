
namespace TEC.FullService.Api.Domain
{
    /// <summary>
    /// Repræsenterer en log-entry for en afsendt notifikation.
    /// Bruges til fejlfinding og sporing.
    /// </summary>
    public class NotificationLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        // public NotificationType NotificationType { get; set; }
        public string Recipient { get; set; } = null!;
        // public NotificationLogStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public string TemplateTypeUsed { get; set; } = null!;
    }
}
