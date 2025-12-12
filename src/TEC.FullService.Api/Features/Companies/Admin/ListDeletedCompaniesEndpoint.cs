using System.Linq.Expressions;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Features.Companies.Mappings;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common.Paging;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Validator
internal sealed class ListDeletedCompaniesValidator : Validator<ListDeletedCompaniesRequest>
{
    public ListDeletedCompaniesValidator()
    {
        RuleFor(x => x.Query.Paging.Page)
            .GreaterThan(0);

        RuleFor(x => x.Query.Paging.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(250);
    }
}

// Swagger summary
internal sealed class ListDeletedCompaniesSummary : Summary<ListDeletedCompaniesEndpoint>
{
    public ListDeletedCompaniesSummary()
    {
        Summary = "Lister soft-slettede virksomheder (admin).";
        Description = "Returnerer en pagineret liste af soft-slettede virksomheder med search og sortering.";

        Response<PagedResponse<CompanyListResponse>>();
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class ListDeletedCompaniesEndpoint(FullServiceDbContext db)
    : ListDeletedEndpointBase<ListDeletedCompaniesRequest, Company, CompanyListResponse>(db)
{
    public override void Configure()
    {
        Get("/admin/companies/deleted");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    // Feature-specifik filtrering
    protected override IQueryable<Company> Query(ListDeletedCompaniesRequest req, IQueryable<Company> query)
    {
        return query;
    }

    // Feature-specifik search
    protected override IQueryable<Company> ApplySearch(IQueryable<Company> q, string search)
    {
        return q.Where(c =>
            c.Name.Contains(search) ||
            c.CvrNumber.Contains(search) ||
            c.ContactEmail.Contains(search));
    }

    protected override Expression<Func<Company, CompanyListResponse>> Projection =>
        CompanyMappings.ToListResponse;
}
