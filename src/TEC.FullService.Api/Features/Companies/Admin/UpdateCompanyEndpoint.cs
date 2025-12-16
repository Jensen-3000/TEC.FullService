using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Validator
internal sealed class UpdateCompanyValidator : Validator<UpdateCompanyRequest>
{
    public UpdateCompanyValidator()
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
internal sealed class UpdateCompanySummary : Summary<UpdateCompanyEndpoint>
{
    public UpdateCompanySummary()
    {
        Summary = "Opdaterer en virksomhed.";
        Description = "Udf�rer en fuld opdatering af virksomheden og returnerer den opdaterede model.";

        Response<CompanyUpdatedResponse>();
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class UpdateCompanyEndpoint(FullServiceDbContext db)
    : UpdateEndpointBase<UpdateCompanyRequest, Company, CompanyUpdatedResponse>(db)
{
    public override void Configure()
    {
        Put("/admin/companies/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(UpdateCompanyRequest req, CancellationToken ct)
    {
        return Db.Companies.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override void UpdateEntity(Company entity, UpdateCompanyRequest req)
    {
        entity.Update(
            req.Name,
            req.CvrNumber,
            req.ContactEmail,
            req.PhoneNumber,
            req.Address
        );
    }

    protected override CompanyUpdatedResponse MapToResponse(Company c)
    {
        return new CompanyUpdatedResponse(
            c.Id,
            c.Name,
            c.CvrNumber,
            c.ContactEmail
        );
    }
}
