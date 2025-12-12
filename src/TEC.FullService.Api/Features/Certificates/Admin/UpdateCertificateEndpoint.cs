using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common;
using TEC.FullService.Shared.DTOs.Certificates.Admin;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Validator
internal sealed class UpdateCertificateValidator : Validator<UpdateCertificateRequest>
{
    public UpdateCertificateValidator()
    {
        RuleFor(x => x.CertificateName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.IssueDate)
            .NotEmpty();

        RuleFor(x => x.ExpiryDate)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate);
    }
}

// Summary
internal sealed class UpdateCertificateSummary : Summary<UpdateCertificateEndpoint>
{
    public UpdateCertificateSummary()
    {
        Summary = "Full update of a certificate.";

        Response<CertificateUpdatedResponse>();
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
        Response(StatusCodes.Status400BadRequest);
    }
}

// Endpoint
internal sealed class UpdateCertificateEndpoint(FullServiceDbContext db)
    : UpdateEndpointBase<UpdateCertificateRequest, Certificate, CertificateUpdatedResponse>(db)
{
    public override void Configure()
    {
        Put("/admin/certificates/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Task<Certificate?> FindEntity(UpdateCertificateRequest req, CancellationToken ct)
    {
        return Db.Certificates.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override void UpdateEntity(Certificate entity, UpdateCertificateRequest req)
    {
        entity.Update(
            req.CertificateName,
            req.IssueDate,
            req.ExpiryDate,
            req.Status,
            req.UserId,
            req.CourseId,
            req.EnrollmentId,
            req.ReplacesId,
            req.CompetenceFundId
        );
    }

    protected override CertificateUpdatedResponse MapToResponse(Certificate entity)
    {
        return new CertificateUpdatedResponse(
            entity.Id,
            entity.CertificateName,
            entity.IssueDate,
            entity.ExpiryDate,
            entity.Status
        );
    }
}
