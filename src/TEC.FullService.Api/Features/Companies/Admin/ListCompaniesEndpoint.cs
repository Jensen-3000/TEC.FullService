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
internal sealed class ListCompaniesValidator : Validator<ListCompaniesRequest>
{
    public ListCompaniesValidator()
    {
        RuleFor(x => x.Query.Paging.Page)
            .GreaterThan(0);

        RuleFor(x => x.Query.Paging.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}

// Swagger summary
internal sealed class ListCompaniesSummary : Summary<ListCompaniesEndpoint>
{
    public ListCompaniesSummary()
    {
        Summary = "Lister virksomheder (admin).";
        Description = "Returnerer en pagineret liste af virksomheder med search, filtrering og sortering.";

        Response<PagedResponse<CompanyListResponse>>();
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class ListCompaniesEndpoint(FullServiceDbContext db)
    : ListEndpointBase<ListCompaniesRequest, Company, CompanyListResponse, CompanySortBy>(db)
{
    public override void Configure()
    {
        Get("/admin/companies");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override IQueryable<Company> Query(ListCompaniesRequest req, IQueryable<Company> query)
    {
        return query;
    }

    protected override IQueryable<Company> ApplySearch(IQueryable<Company> query, string search)
    {
        search = search.Trim();

        return query.Where(c =>
            c.Name.Contains(search) ||
            c.CvrNumber.Contains(search) ||
            c.ContactEmail.Contains(search));
    }

    protected override Expression<Func<Company, CompanyListResponse>> Projection =>
        CompanyMappings.ToListResponse;
}
