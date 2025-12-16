using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Companies.Admin;

/// Summary
internal sealed class RestoreCompanySummary : Summary<RestoreCompanyEndpoint>
{
    public RestoreCompanySummary()
    {
        Summary = "Genskaber en soft-slettet virksomhed.";
        Description = "Sætter IsDeleted til false og rydder DeletedAt/DeletedBy.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}


// Endpoint
internal sealed class RestoreCompanyEndpoint(FullServiceDbContext db)
    : RestoreEndpointBase<Company>(db)
{
    public override void Configure()
    {
        Patch("/admin/companies/{id:guid}/restore");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(Guid id, CancellationToken ct)
    {
        return Db.Companies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, ct);
    }
}
