using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Validator
internal sealed class CreateCompanyValidator : Validator<CreateCompanyRequest>
{
    public CreateCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.CvrNumber)
            .NotEmpty()
            .Length(8)
            .Matches("^[0-9]{8}$");

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .MaximumLength(500);
    }
}

// Summary for Swagger
internal sealed class CreateCompanySummary : Summary<CreateCompanyEndpoint>
{
    public CreateCompanySummary()
    {
        Summary = "Opret virksomhed (admin).";
        Description = "Opretter en ny virksomhed og returnerer dens data.";

        Response<CompanyCreatedResponse>(StatusCodes.Status201Created);
    }
}

// Endpoint
internal sealed class CreateCompanyEndpoint(FullServiceDbContext db)
    : CreateEndpointBase<CreateCompanyRequest, Company, CompanyCreatedResponse>(db)
{
    public override void Configure()
    {
        Post("/admin/companies");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Company CreateEntity(CreateCompanyRequest req)
    {
        return Company.Create(
            req.Name,
            req.CvrNumber,
            req.ContactEmail,
            req.PhoneNumber,
            req.Address
        );
    }

    protected override CompanyCreatedResponse MapToResponse(Company c)
    {
        return new CompanyCreatedResponse(
            c.Id,
            c.Name,
            c.CvrNumber,
            c.ContactEmail
        );
    }
}
