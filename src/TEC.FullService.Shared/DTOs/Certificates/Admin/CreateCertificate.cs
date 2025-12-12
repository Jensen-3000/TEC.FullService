using TEC.FullService.Shared.Common;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class CreateCertificateRequest
{
    public required string CertificateName { get; set; }
    public required DateTime IssueDate { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public required CertificateStatus Status { get; set; }
    public required int UserId { get; set; }
    public required int CourseId { get; set; }
    public int? EnrollmentId { get; set; }
    public int? ReplacesId { get; set; }
    public int? CompetenceFundId { get; set; }
}

// Response
public sealed record CertificateCreatedResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);
