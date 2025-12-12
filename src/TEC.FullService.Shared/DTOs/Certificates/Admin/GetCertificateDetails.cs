using TEC.FullService.Shared.Common;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class GetCertificateDetailsRequest
{
    public Guid Id { get; set; }
}

// Response
public sealed record CertificateDetailsResponse(
    Guid Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status,
    Guid UserId,
    Guid CourseId,
    Guid? EnrollmentId,
    Guid? ReplacesId,
    Guid? CompetenceFundId,
    string UserName,
    string? CompanyName,
    EducationStatus? EducationStatus,
    string? CompetenceFundName,
    string CourseName);
