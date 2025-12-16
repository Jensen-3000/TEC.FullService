using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Certificates.Admin;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Validator
internal sealed class HardDeleteCertificateValidator : Validator<HardDeleteCertificateRequest>
{
    public HardDeleteCertificateValidator() { }
}

// Summary for Swagger
internal sealed class HardDeleteCertificateSummary : Summary<HardDeleteCertificateEndpoint>
{
    public HardDeleteCertificateSummary()
    {
        Summary = "Hard delete certificate (admin).";
        Description = "Permanently removes a certificate from the database. This operation ignores soft-delete filters and cannot be undone.";
        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class HardDeleteCertificateEndpoint(FullServiceDbContext db)
    : HardDeleteEndpointBase<HardDeleteCertificateRequest, Certificate>(db)
{
    public override void Configure()
    {
        Delete("/admin/certificates/{id:guid}/hard-delete");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Task<Certificate?> FindEntity(HardDeleteCertificateRequest req, CancellationToken ct)
    {
        return Db.Certificates
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }
}
