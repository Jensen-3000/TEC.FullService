using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Summary for Swagger
internal sealed class RestoreCertificateSummary : Summary<RestoreCertificateEndpoint>
{
    public RestoreCertificateSummary()
    {
        Summary = "Restores a soft-deleted certificate.";
        Description = "Sets IsDeleted to false and clears DeletedAt/DeletedBy.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class RestoreCertificateEndpoint(FullServiceDbContext db)
    : RestoreEndpointBase<Certificate>(db)
{
    public override void Configure()
    {
        Patch("/admin/certificates/{id:guid}/restore");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Task<Certificate?> FindEntity(Guid id, CancellationToken ct)
    {
        return Db.Certificates
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, ct);
    }
}
