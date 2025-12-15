using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Domain;

public sealed record CertificatePatch(
    string? CertificateName = null,
    DateTime? IssueDate = null,
    DateTime? ExpiryDate = null,
    CertificateStatus? Status = null,
    Guid? UserId = null,
    Guid? CourseId = null,
    Guid? EnrollmentId = null,
    Guid? ReplacesId = null,
    Guid? CompetenceFundId = null);
