namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class GetCertificateDetailsRequest
{
    public int Id { get; set; }
}

// Response
public sealed record CertificateDetailsResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status,
    int UserId,
    int CourseId,
    int? EnrollmentId,
    int? ReplacesId,
    int? CompetenceFundId,
    string UserName,
    string? CompanyName,
    EducationStatus? EducationStatus,
    string? CompetenceFundName,
    string CourseName);
