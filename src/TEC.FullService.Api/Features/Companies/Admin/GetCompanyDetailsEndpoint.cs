using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Companies.Mappings;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

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
