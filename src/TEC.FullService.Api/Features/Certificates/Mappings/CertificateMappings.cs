using System.Linq.Expressions;
using TEC.FullService.Api.Domain;
using TEC.FullService.Shared.DTOs.Certificates.Admin;

namespace TEC.FullService.Api.Features.Certificates.Mappings;

public static class CertificateMappings
{
    // List projection
    internal static readonly Expression<Func<Certificate, CertificateListResponse>> ToListResponse =
        c => new CertificateListResponse(
            c.Id,
            c.CertificateName,
            c.IssueDate,
            c.ExpiryDate,
            c.Status.ToString(),
            c.User.DisplayName,
            c.User.Company != null ? c.User.Company.Name : null,
            c.Course.Name);

    // Details projection
    internal static readonly Expression<Func<Certificate, CertificateDetailsResponse>> ToDetailsResponse =
        c => new CertificateDetailsResponse(
            c.Id,
            c.CertificateName,
            c.IssueDate,
            c.ExpiryDate,
            c.Status,
            c.UserId,
            c.CourseId,
            c.EnrollmentId,
            c.ReplacesId,
            c.CompetenceFundId,
            c.User.DisplayName,
            // Company may be null
            c.User.Company != null ? c.User.Company.Name : null,
            c.User.EducationStatus,
            // CompetenceFund may be null
            c.CompetenceFund != null ? c.CompetenceFund.Name : null,
            c.Course.Name);
}
