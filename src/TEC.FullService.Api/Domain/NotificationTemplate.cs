namespace TEC.FullService.Api.Domain;

/// <summary>
/// Repræsenterer en skabelon for en notifikation (e-mail eller SMS).
/// Giver administratorer mulighed for at redigere kommunikationen.
/// </summary>
public class NotificationTemplate
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    /// <summary>
    /// Et unikt systemnavn for skabelonen, f.eks. "CertificateExpiry6Months".
    /// Bruges af koden til at finde den rigtige skabelon.
    /// </summary>
    public string TemplateType { get; set; } = null!;

    public string EmailSubject { get; set; } = null!;

    public string EmailBody { get; set; } = null!;

    public string? SmsBody { get; set; }
}
