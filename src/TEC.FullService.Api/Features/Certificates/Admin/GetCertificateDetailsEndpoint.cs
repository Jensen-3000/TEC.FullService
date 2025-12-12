using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Certificates.Mappings;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Request
internal sealed class GetCertificateDetailsRequest
{
    public int Id { get; set; }
}

// Response
internal sealed record CertificateDetailsResponse(
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

// Summary for Swagger
internal class GetCertificateDetailsSummary : Summary<GetCertificateDetailsEndpoint>
{
    public GetCertificateDetailsSummary()
    {
        Summary = "Get certificate details";
        Description = "Returns detailed information about a specific certificate including user, company, and course details.";
        Response<CertificateDetailsResponse>();
        Response(StatusCodes.Status401Unauthorized);
        Response(StatusCodes.Status403Forbidden);
        Response(StatusCodes.Status404NotFound);
    }
}

// Endpoint
internal sealed class GetCertificateDetailsEndpoint(FullServiceDbContext db)
    : DetailsEndpointBase<GetCertificateDetailsRequest, Certificate, CertificateDetailsResponse>(db)
{
    public override void Configure()
    {
        Get("/admin/certificates/{id}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override IQueryable<Certificate> Query(GetCertificateDetailsRequest req)
    {
        return Db.Certificates
            .Where(c => c.Id == req.Id)
            .AsNoTracking();
    }

    protected override Expression<Func<Certificate, CertificateDetailsResponse>> Projection =>
        CertificateMappings.ToDetailsResponse;
}
