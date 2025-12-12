using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Request
internal sealed class PatchCertificateRequest
{
    public int Id { get; set; }
    public string? CertificateName { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public CertificateStatus? Status { get; set; }
    public int? UserId { get; set; }
    public int? CourseId { get; set; }
    public int? EnrollmentId { get; set; }
    public int? ReplacesId { get; set; }
    public int? CompetenceFundId { get; set; }
}

// Response
internal sealed record CertificatePatchedResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);

// Validator
internal sealed class PatchCertificateValidator : Validator<PatchCertificateRequest>
{
    public PatchCertificateValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.CertificateName)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.CertificateName is not null);

        RuleFor(x => x.IssueDate)
            .NotEmpty()
            .When(x => x.IssueDate.HasValue);

        RuleFor(x => x.ExpiryDate)
            .NotEmpty()
            .When(x => x.ExpiryDate.HasValue);

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .When(x => x.UserId.HasValue);

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .When(x => x.CourseId.HasValue);

        RuleFor(x => x.EnrollmentId)
            .GreaterThan(0)
            .When(x => x.EnrollmentId.HasValue);

        RuleFor(x => x.ReplacesId)
            .GreaterThan(0)
            .When(x => x.ReplacesId.HasValue);

        RuleFor(x => x.CompetenceFundId)
            .GreaterThan(0)
            .When(x => x.CompetenceFundId.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status.HasValue);

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate)
            .When(x => x.IssueDate.HasValue && x.ExpiryDate.HasValue);
    }
}

// Summary for Swagger
internal sealed class PatchCertificateSummary : Summary<PatchCertificateEndpoint>
{
    public PatchCertificateSummary()
    {
        Summary = "Patch certificate (partial update)";
        Description = "Updates only the specified fields of a certificate. Send only the fields you want to update.";
        ExampleRequest = new PatchCertificateRequest
        {
            Id = 1,
            CertificateName = "Updated Name Only"
        };
        Response<CertificatePatchedResponse>();
        Response(StatusCodes.Status400BadRequest);
        Response(StatusCodes.Status401Unauthorized);
        Response(StatusCodes.Status403Forbidden);
        Response(StatusCodes.Status404NotFound);
    }
}

// Endpoint
internal sealed class PatchCertificateEndpoint(FullServiceDbContext db)
    : PatchEndpointBase<PatchCertificateRequest, Certificate, CertificatePatchedResponse>(db)
{
    public override void Configure()
    {
        Patch("/admin/certificates/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Task<Certificate?> FindEntity(PatchCertificateRequest req, CancellationToken ct)
    {
        return Db.Certificates.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override void ApplyPatch(Certificate entity, PatchCertificateRequest req)
    {
        entity.ApplyPatch(req);
    }

    protected override CertificatePatchedResponse MapToResponse(Certificate entity)
    {
        return new CertificatePatchedResponse(
            entity.Id,
            entity.CertificateName,
            entity.IssueDate,
            entity.ExpiryDate,
            entity.Status
        );
    }
}
