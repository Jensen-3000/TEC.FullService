namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class UpdateCertificateRequest
{
    public int Id { get; set; }
    public required string CertificateName { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public CertificateStatus Status { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int? EnrollmentId { get; set; }
    public int? ReplacesId { get; set; }
    public int? CompetenceFundId { get; set; }
}

// Response
public sealed record CertificateUpdatedResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);
