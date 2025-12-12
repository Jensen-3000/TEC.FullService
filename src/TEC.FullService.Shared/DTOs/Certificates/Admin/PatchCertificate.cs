using TEC.FullService.Shared.Common;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class PatchCertificateRequest
{
    public Guid Id { get; set; }
    public string? CertificateName { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public CertificateStatus? Status { get; set; }
    public Guid? UserId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? EnrollmentId { get; set; }
    public Guid? ReplacesId { get; set; }
    public Guid? CompetenceFundId { get; set; }
}

// Response
public sealed record CertificatePatchedResponse(
    Guid Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);
