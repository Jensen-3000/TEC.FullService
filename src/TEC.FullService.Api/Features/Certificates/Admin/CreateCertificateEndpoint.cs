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
            UserId = Guid.CreateVersion7(),
            CourseId = Guid.CreateVersion7(),
            CompetenceFundId = Guid.CreateVersion7()
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
