using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Domain;

/// <summary>
///     Repræsenterer et certifikat, en bruger har opnået.
///     Kan oprettes automatisk efter en gennemført tilmelding eller manuelt.
/// </summary>
public class Certificate : SoftDeletableEntity
{
    private Certificate() { }

    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public string CertificateName { get; private set; } = null!;
    public DateTime IssueDate { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public CertificateStatus Status { get; private set; }

    // Fremmednøgler
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public Guid? EnrollmentId { get; set; } // Nullable, for manuelt oprettede certifikater
    public Guid? ReplacesId { get; set; } // Nullable, for fornyelseshistorik
    public Guid? CompetenceFundId { get; set; }

    // Navigation properties
    public UserProfile User { get; set; } = null!;
    public Course Course { get; set; } = null!;
    //public Enrollment? Enrollment { get; set; } // behøver vi overhovedet denne?
    public Certificate? ReplacedCertificate { get; set; }
    public CompetenceFund? CompetenceFund { get; set; }

    public static Certificate Create(
        string certificateName,
        DateTime issueDate,
        DateTime expiryDate,
        CertificateStatus status,
        Guid userId,
        Guid courseId,
        Guid? enrollmentId = null,
        Guid? replacesId = null,
        Guid? competenceFundId = null)
    {
        ValidateDates(issueDate, expiryDate);
        ValidateName(certificateName);

        return new Certificate
        {
            Id = Guid.CreateVersion7(),
            CertificateName = certificateName,
            IssueDate = issueDate,
            ExpiryDate = expiryDate,
            Status = status,
            UserId = userId,
            CourseId = courseId,
            EnrollmentId = enrollmentId,
            ReplacesId = replacesId,
            CompetenceFundId = competenceFundId
        };
    }

    public void Update(
        string certificateName,
        DateTime issueDate,
        DateTime expiryDate,
        CertificateStatus status,
        Guid userId,
        Guid courseId,
        Guid? enrollmentId,
        Guid? replacesId,
        Guid? competenceFundId)
    {
        ValidateDates(issueDate, expiryDate);
        ValidateName(certificateName);
        ValidateReplacesId(replacesId);

        CertificateName = certificateName;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        Status = status;
        UserId = userId;
        CourseId = courseId;
        EnrollmentId = enrollmentId;
        ReplacesId = replacesId;
        CompetenceFundId = competenceFundId;
    }

    /// <summary>
    /// Applies a patch to the certificate, updating only the fields provided in the request.
    /// </summary>
    public void ApplyPatch(Patch patch)
    {
        if (patch.CertificateName is not null)
        {
            ValidateName(patch.CertificateName);
            CertificateName = patch.CertificateName;
        }

        if (patch.IssueDate.HasValue)
        {
            // Hvis vi opdaterer IssueDate, skal vi validere ift. enten ny ExpiryDate eller eksisterende.
            var newIssue = patch.IssueDate.Value;
            var newExpiry = patch.ExpiryDate ?? ExpiryDate;
            ValidateDates(newIssue, newExpiry);
            IssueDate = newIssue;
        }

        if (patch.ExpiryDate.HasValue)
        {
            var newIssue = patch.IssueDate ?? IssueDate;
            var newExpiry = patch.ExpiryDate.Value;
            ValidateDates(newIssue, newExpiry);
            ExpiryDate = newExpiry;
        }

        if (patch.Status.HasValue)
            Status = patch.Status.Value;

        if (patch.UserId.HasValue)
            UserId = patch.UserId.Value;

        if (patch.CourseId.HasValue)
            CourseId = patch.CourseId.Value;

        if (patch.EnrollmentId.HasValue)
            EnrollmentId = patch.EnrollmentId.Value;

        if (patch.ReplacesId.HasValue)
        {
            ValidateReplacesId(patch.ReplacesId.Value);
            ReplacesId = patch.ReplacesId.Value;
        }

        if (patch.CompetenceFundId.HasValue)
            CompetenceFundId = patch.CompetenceFundId.Value;
    }

    private static void ValidateDates(DateTime issueDate, DateTime expiryDate)
    {
        if (issueDate >= expiryDate)
            throw new DomainException("Issue date must be before expiry date.");
    }

    private static void ValidateName(string certificateName)
    {
        if (string.IsNullOrWhiteSpace(certificateName))
            throw new DomainException("Certificate name is required");

        if (certificateName.Length > 255)
            throw new DomainException("Certificate name cannot exceed 255 characters.");
    }

    private void ValidateReplacesId(Guid? replacesId)
    {
        if (replacesId.HasValue && replacesId.Value == Id)
            throw new DomainException("A certificate cannot replace itself.");
    }

    /// <summary>
    /// Represents a partial update to a Certificate entity.
    /// Used for PATCH operations in the domain layer.
    /// </summary>
    public sealed record Patch(
        string? CertificateName = null,
        DateTime? IssueDate = null,
        DateTime? ExpiryDate = null,
        CertificateStatus? Status = null,
        Guid? UserId = null,
        Guid? CourseId = null,
        Guid? EnrollmentId = null,
        Guid? ReplacesId = null,
        Guid? CompetenceFundId = null);
}
