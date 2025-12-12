using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Certificates.Mappings;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common.Paging;
using TEC.FullService.Shared.DTOs.Certificates.Admin;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Validator
internal sealed class ListDeletedCertificatesValidator : Validator<ListDeletedCertificatesRequest>
{
    public ListDeletedCertificatesValidator()
    {
        RuleFor(x => x.Query.Paging.Page)
            .GreaterThan(0);

        RuleFor(x => x.Query.Paging.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(250);
    }
}

// Summary for Swagger
internal sealed class ListDeletedCertificatesSummary : Summary<ListDeletedCertificatesEndpoint>
{
    public ListDeletedCertificatesSummary()
    {
        Summary = "Lister soft-slettede certifikater (admin).";
        Description = "Returnerer en pagineret liste af soft-slettede certifikater med search og sortering.";

        Response<PagedResponse<CertificateListResponse>>();
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class ListDeletedCertificatesEndpoint(FullServiceDbContext db)
    : ListDeletedEndpointBase<ListDeletedCertificatesRequest, Certificate, CertificateListResponse>(db)
{
    public override void Configure()
    {
        Get("/admin/certificates/deleted");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override IQueryable<Certificate> Query(ListDeletedCertificatesRequest req, IQueryable<Certificate> query)
    {
        return query;
    }

    protected override IQueryable<Certificate> ApplySearch(IQueryable<Certificate> q, string search)
    {
        return q.Where(c =>
            c.CertificateName.Contains(search) ||
            c.User.FirstName.Contains(search) ||
            c.User.LastName.Contains(search) ||
            c.Course.Name.Contains(search)
        );
    }

    // Projection
    protected override Expression<Func<Certificate, CertificateListResponse>> Projection =>
        CertificateMappings.ToListResponse;
}
