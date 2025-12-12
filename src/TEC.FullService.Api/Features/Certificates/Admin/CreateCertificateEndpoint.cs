using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Features.Certificates.Admin;

// Request
internal sealed class CreateCertificateRequest
{
    public required string CertificateName { get; set; }
    public required DateTime IssueDate { get; set; }
    public required DateTime ExpiryDate { get; set; }
    public required CertificateStatus Status { get; set; }
    public required int UserId { get; set; }
    public required int CourseId { get; set; }
    public int? EnrollmentId { get; set; }
    public int? ReplacesId { get; set; }
    public int? CompetenceFundId { get; set; }
}

// Response
internal sealed record CertificateCreatedResponse(
    int Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    CertificateStatus Status);

// Validator
internal class CreateCertificateValidator : Validator<CreateCertificateRequest>
{
    public CreateCertificateValidator()
    {
        RuleFor(x => x.CertificateName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.IssueDate)
            .NotEmpty();

        RuleFor(x => x.ExpiryDate)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .GreaterThan(0);

        RuleFor(x => x.CourseId)
            .GreaterThan(0);

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
            .IsInEnum();

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate);
    }
}

// Summary for Swagger
internal class CreateCertificateSummary : Summary<CreateCertificateEndpoint>
{
    public CreateCertificateSummary()
    {
        Summary = "Create certificate";
        Description = "Creates a new certificate and returns the created certificate details.";
        ExampleRequest = new CreateCertificateRequest
        {
            CertificateName = "Varmt Arbejde Certifikat",
            IssueDate = new DateTime(2025, 1, 1),
            ExpiryDate = new DateTime(2027, 1, 1),
            Status = CertificateStatus.Active,
            UserId = 1,
            CourseId = 1,
            CompetenceFundId = 1
        };
        Response<CertificateCreatedResponse>(StatusCodes.Status201Created);
        Response(StatusCodes.Status400BadRequest);
        Response(StatusCodes.Status401Unauthorized);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class CreateCertificateEndpoint(FullServiceDbContext db)
    : CreateEndpointBase<CreateCertificateRequest, Certificate, CertificateCreatedResponse>(db)
{
    public override void Configure()
    {
        Post("/admin/certificates");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Certificates"));
    }

    protected override Certificate CreateEntity(CreateCertificateRequest req)
    {
        return Certificate.Create(
            req.CertificateName,
            req.IssueDate,
            req.ExpiryDate,
            req.Status,
            req.UserId,
            req.CourseId,
            req.EnrollmentId,
            req.ReplacesId,
            req.CompetenceFundId);
    }

    protected override CertificateCreatedResponse MapToResponse(Certificate c)
    {
        return new CertificateCreatedResponse(
            c.Id,
            c.CertificateName,
            c.IssueDate,
            c.ExpiryDate,
            c.Status
        );
    }
}
