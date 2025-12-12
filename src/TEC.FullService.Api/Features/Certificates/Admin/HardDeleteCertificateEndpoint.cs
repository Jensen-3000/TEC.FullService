using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Request
internal sealed class HardDeleteCertificateRequest
{
    public int Id { get; set; }
}

// Validator
internal sealed class HardDeleteCertificateValidator : Validator<HardDeleteCertificateRequest>
{
    public HardDeleteCertificateValidator() => RuleFor(x => x.Id).GreaterThan(0);
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
        Delete("/admin/certificates/{id}/hard-delete");
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
