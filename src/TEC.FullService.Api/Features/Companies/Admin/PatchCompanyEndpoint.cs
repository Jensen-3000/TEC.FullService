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
internal sealed class PatchCompanyValidator : Validator<PatchCompanyRequest>
{
    public PatchCompanyValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.Name is not null);

        RuleFor(x => x.CvrNumber)
            .NotEmpty()
            .Length(8)
            .Matches("^[0-9]{8}$")
            .When(x => x.CvrNumber is not null);

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255)
            .When(x => x.ContactEmail is not null);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(50)
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => x.Address is not null);
    }
}

// Summary for Swagger
internal sealed class PatchCompanySummary : Summary<PatchCompanyEndpoint>
{
    public PatchCompanySummary()
    {
        Summary = "Partial update af virksomhed (admin).";
        Description = "Opdaterer kun de felter der sendes med.";

        Response<CompanyPatchedResponse>();
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
        Response(StatusCodes.Status400BadRequest);
    }
}

// Endpoint
internal sealed class PatchCompanyEndpoint(FullServiceDbContext db)
    : PatchEndpointBase<PatchCompanyRequest, Company, CompanyPatchedResponse>(db)
{
    public override void Configure()
    {
        Patch("/admin/companies/{id}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(PatchCompanyRequest req, CancellationToken ct)
    {
        return Db.Companies.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }

    protected override void ApplyPatch(Company entity, PatchCompanyRequest req)
    {
        entity.ApplyPatch(new CompanyPatch(
            Name: req.Name,
            CvrNumber: req.CvrNumber,
            ContactEmail: req.ContactEmail,
            PhoneNumber: req.PhoneNumber,
            Address: req.Address));
    }

    protected override CompanyPatchedResponse MapToResponse(Company c)
    {
        return new CompanyPatchedResponse(
            c.Id,
            c.Name,
            c.CvrNumber,
            c.ContactEmail
        );
    }
}
