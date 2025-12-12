using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Certificates.Mappings;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common.Paging;

namespace TEC.FullService.Api.Features.Certificates.Admin;

internal enum CertificateSortBy
{
    Id,
    IssueDate,
    ExpiryDate,
    Status
}

// Request
internal sealed class ListCertificatesRequest : IHasListQuery, ISortRequest<CertificateSortBy>
{
    public ListQuery Query { get; set; } = new();
    public CertificateSortBy? SortBy { get; set; }
    public bool? Desc { get; set; }
}

// Response
internal sealed record CertificateListResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    string Status,
    string UserName,
    string? CompanyName,
    string CourseName
);

// Validator
internal class ListCertificatesValidator : Validator<ListCertificatesRequest>
{
    public ListCertificatesValidator()
    {
        RuleFor(x => x.Query.Paging.Page)
            .GreaterThan(0);

        RuleFor(x => x.Query.Paging.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}

// Summary for Swagger
internal class ListCertificatesSummary : Summary<ListCertificatesEndpoint>
{
    public ListCertificatesSummary()
    {
        Summary = "List certificates (paginated)";
        Description = "Returns a paginated list of certificates with user, company, and course details.";
        ExampleRequest = new ListCertificatesRequest();
        Response<PagedResponse<CertificateListResponse>>();
        Response(StatusCodes.Status400BadRequest);
        Response(StatusCodes.Status401Unauthorized);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class ListCertificatesEndpoint(FullServiceDbContext db)
    : ListEndpointBase<ListCertificatesRequest, Certificate, CertificateListResponse, CertificateSortBy>(db)
{
    public override void Configure()
    {
        Get("/admin/certificates");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override IQueryable<Certificate> Query(ListCertificatesRequest req, IQueryable<Certificate> query)
    {
        return query;
    }

    protected override IQueryable<Certificate> ApplySearch(IQueryable<Certificate> query, string search)
    {
        return query.Where(c =>
            c.CertificateName.Contains(search) ||
            c.User.FirstName.Contains(search) ||
            c.User.LastName.Contains(search) ||
            c.Course.Name.Contains(search));
    }

    // Projection
    protected override Expression<Func<Certificate, CertificateListResponse>> Projection
        => CertificateMappings.ToListResponse;
}
