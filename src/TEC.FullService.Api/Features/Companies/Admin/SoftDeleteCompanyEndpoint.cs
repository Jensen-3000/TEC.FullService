using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.BaseEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.Companies.Admin;

// Request
internal sealed class SoftDeleteCompanyRequest
{
    public Guid Id { get; set; }
}

// Validator
internal sealed class SoftDeleteCompanyValidator : Validator<SoftDeleteCompanyRequest>
{
    public SoftDeleteCompanyValidator()
    {

    }
}

// Summary (Swagger)
internal sealed class SoftDeleteCompanySummary : Summary<SoftDeleteCompanyEndpoint>
{
    public SoftDeleteCompanySummary()
    {
        Summary = "Soft-delete virksomhed (admin).";
        Description = "Markér virksomheden som slettet via domænemetoden. " +
                      "Data bevares og kan genskabes via admins deleted-list.";

        Response(StatusCodes.Status204NoContent);
        Response(StatusCodes.Status404NotFound);
        Response(StatusCodes.Status403Forbidden);
    }
}

// Endpoint
internal sealed class SoftDeleteCompanyEndpoint(FullServiceDbContext db)
    : SoftDeleteEndpointBase<SoftDeleteCompanyRequest, Company>(db)
{
    public override void Configure()
    {
        Delete("/admin/companies/{id:guid}");
        Roles("Admin");
        Description(x => x.AutoTagOverride("Admin/Companies"));
    }

    protected override Task<Company?> FindEntity(SoftDeleteCompanyRequest req, CancellationToken ct)
    {
        return Db.Companies.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
    }
}
