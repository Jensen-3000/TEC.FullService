using FastEndpoints;
using FastEndpoints.Swagger;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Validator
internal sealed class HardDeleteCompanyValidator : Validator<HardDeleteCompanyRequest>
{
    public HardDeleteCompanyValidator()
    {

    }
}

// Summary for Swagger
internal sealed class HardDeleteCompanySummary : Summary<HardDeleteCompanyEndpoint>
{
    public HardDeleteCompanySummary()
    {
        Summary = "Hard delete (permanent) af virksomhed.";
        Description = "Fjerner virksomheden fysisk fra databasen. Kan ikke fortrydes.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class HardDeleteCompanyEndpoint(FullServiceDbContext db)
    : HardDeleteEndpointBase<HardDeleteCompanyRequest, Company>(db)
{
    public override void Configure()
    {
        Delete("/admin/companies/{id:guid}/hard-delete");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(HardDeleteCompanyRequest req, CancellationToken ct)
    {
        return Db.Companies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override async Task<bool> ValidateDeleteAsync(HardDeleteCompanyRequest req, Company entity, CancellationToken ct)
    {
        var hasUsers = await Db.Set<UserProfile>().AnyAsync(u => u.CompanyId == entity.Id, ct);
        if (!hasUsers)
            return true;

        AddError("company", "Company cannot be deleted while users exist.");
        return false;
    }
}
