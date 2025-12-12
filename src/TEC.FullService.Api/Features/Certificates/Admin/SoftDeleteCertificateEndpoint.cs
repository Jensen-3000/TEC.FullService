using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Request
internal sealed class SoftDeleteCertificateRequest
{
    public int Id { get; set; }
}

// Validator
internal sealed class SoftDeleteCertificateValidator : Validator<SoftDeleteCertificateRequest>
{
    public SoftDeleteCertificateValidator() { }
}

// Summary for Swagger
internal sealed class SoftDeleteCertificateSummary : Summary<SoftDeleteCertificateEndpoint>
{
    public SoftDeleteCertificateSummary()
    {
        Summary = "Soft-delete certificate (admin).";
        Description = "Mark a certificate as deleted using the domain method. Data is preserved and can be restored by admins from the deleted list.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class SoftDeleteCertificateEndpoint(FullServiceDbContext db)
    : SoftDeleteEndpointBase<SoftDeleteCertificateRequest, Certificate>(db)
{
    public override void Configure()
    {
        Delete("/admin/certificates/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Task<Certificate?> FindEntity(SoftDeleteCertificateRequest req, CancellationToken ct)
    {
        return Db.Certificates.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }
}
