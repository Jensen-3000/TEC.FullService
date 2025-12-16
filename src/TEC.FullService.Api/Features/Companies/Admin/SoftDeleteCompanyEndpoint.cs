using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Validator
internal sealed class SoftDeleteCompanyValidator : Validator<SoftDeleteCompanyRequest>
{
    public SoftDeleteCompanyValidator()
    {

    }
}

// Summary for Swagger
internal sealed class SoftDeleteCompanySummary : Summary<SoftDeleteCompanyEndpoint>
{
    public SoftDeleteCompanySummary()
    {
        Summary = "Soft-delete virksomhed (admin).";
        Description = "Markér virksomheden som slettet via domænemetoden. " +
                      "Data bevares og kan genskabes via admins deleted-list.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class SoftDeleteCompanyEndpoint(FullServiceDbContext db)
    : SoftDeleteEndpointBase<SoftDeleteCompanyRequest, Company>(db)
{
    public override void Configure()
    {
        Delete("/admin/companies/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(SoftDeleteCompanyRequest req, CancellationToken ct)
    {
        return Db.Companies.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override async Task<bool> ValidateDeleteAsync(SoftDeleteCompanyRequest req, Company entity, CancellationToken ct)
    {
        var hasUsers = await Db.Set<UserProfile>().AnyAsync(u => u.CompanyId == entity.Id, ct);
        if (!hasUsers)
            return true;

        AddError("company", "Company cannot be deleted while users exist.");
        return false;
    }
}
