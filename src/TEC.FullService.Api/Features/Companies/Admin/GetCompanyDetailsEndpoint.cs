using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Companies.Mappings;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Request
internal sealed class GetCompanyDetailsRequest
{
    public Guid Id { get; set; }
}

// Response
internal sealed record CompanyDetailsResponse(
    Guid Id,
    string Name,
    string CvrNumber,
    string ContactEmail,
    string? PhoneNumber,
    string? Address,
    DateTime CreatedAt
);

// Swagger summary
internal sealed class GetCompanyDetailsSummary : Summary<GetCompanyDetailsEndpoint>
{
    public GetCompanyDetailsSummary()
    {
        Summary = "Henter detaljer om en virksomhed (admin).";
        Description = "Returnerer alle relevante detaljer for en virksomhed.";
        Response<CompanyDetailsResponse>();
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class GetCompanyDetailsEndpoint(FullServiceDbContext db)
    : DetailsEndpointBase<GetCompanyDetailsRequest, Company, CompanyDetailsResponse>(db)
{
    public override void Configure()
    {
        Get("/admin/companies/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override IQueryable<Company> Query(GetCompanyDetailsRequest req)
    {
        return Db.Companies.Where(c => c.Id == req.Id).AsNoTracking();
    }

    protected override Expression<Func<Company, CompanyDetailsResponse>> Projection =>
        CompanyMappings.ToDetailsResponse;
}
