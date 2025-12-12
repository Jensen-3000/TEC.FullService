using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Shared.DTOs.Companies.Admin;

namespace TEC.FullService.Api.Domain;

public class Company : SoftDeletableEntity
{
    protected Company()
    {

    }

    public Guid Id { get; private set; }
    public required string Name { get; set; }
    public required string CvrNumber { get; set; }
    public required string ContactEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    public ICollection<UserProfile> Users { get; set; } = new List<UserProfile>();

    /// <summary>
    /// Factory method to create a new Company entity with validation.
    /// </summary>
    public static Company Create(
        string name,
        string cvrNumber,
        string contactEmail,
        string? phoneNumber = null,
        string? address = null)
    {
        ValidateName(name);
        ValidateCvrNumber(cvrNumber);
        ValidateContactEmail(contactEmail);

        return new Company
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            CvrNumber = cvrNumber,
            ContactEmail = contactEmail,
            PhoneNumber = phoneNumber,
            Address = address,
        };
    }

    /// <summary>
    /// Updates the company with new values and validation.
    /// </summary>
    public void Update(
        string name,
        string cvrNumber,
        string contactEmail,
        string? phoneNumber,
        string? address)
    {
        ValidateName(name);
        ValidateCvrNumber(cvrNumber);
        ValidateContactEmail(contactEmail);

        Name = name;
        CvrNumber = cvrNumber;
        ContactEmail = contactEmail;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    /// <summary>
    /// Applies a patch request to the company, updating only provided fields.
    /// </summary>
    /// <param name="req"></param>
    public void ApplyPatch(PatchCompanyRequest req)
    {
        if (req.Name is not null)
        {
            ValidateName(req.Name);
            Name = req.Name;
        }

        if (req.CvrNumber is not null)
        {
            ValidateCvrNumber(req.CvrNumber);
            CvrNumber = req.CvrNumber;
        }

        if (req.ContactEmail is not null)
        {
            ValidateContactEmail(req.ContactEmail);
            ContactEmail = req.ContactEmail;
        }

        if (req.PhoneNumber is not null)
            PhoneNumber = req.PhoneNumber;

        if (req.Address is not null)
            Address = req.Address;
    }

    public override void Delete(Guid? userId = null)
    {
        if (Users.Any()) // Use Any() as its more efficient than Count, as count is in memory and Any can be translated to SQL EXISTS
            throw new DomainException("Company cannot be deleted while users exist.");

        base.Delete(userId);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Company name is required.");

        if (name.Length > 255)
            throw new DomainException("Company name cannot exceed 255 characters.");
    }

    private static void ValidateCvrNumber(string cvrNumber)
    {
        if (string.IsNullOrWhiteSpace(cvrNumber))
            throw new DomainException("CVR number is required.");

        if (cvrNumber.Length != 8 || !cvrNumber.All(char.IsDigit))
            throw new DomainException("CVR number must be exactly 8 digits.");
    }

    private static void ValidateContactEmail(string contactEmail)
    {
        if (string.IsNullOrWhiteSpace(contactEmail))
            throw new DomainException("Contact email is required.");

        if (contactEmail.Length > 255)
            throw new DomainException("Contact email cannot exceed 255 characters.");
    }
}
