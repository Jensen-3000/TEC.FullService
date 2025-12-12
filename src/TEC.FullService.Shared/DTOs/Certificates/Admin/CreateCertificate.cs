using TEC.FullService.Shared.Common;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class CreateCertificateRequest
{
    public required string CertificateName { get; set; }
    public required DateTime IssueDate { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public required CertificateStatus Status { get; set; }
    public required Guid UserId { get; set; }
    public required Guid CourseId { get; set; }
    public Guid? EnrollmentId { get; set; }
    public Guid? ReplacesId { get; set; }
    public Guid? CompetenceFundId { get; set; }
}

// Response
public sealed record CertificateCreatedResponse(
    Guid Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);
