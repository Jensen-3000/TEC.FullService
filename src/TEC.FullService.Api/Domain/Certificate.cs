using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Shared.Common;
using TEC.FullService.Shared.DTOs.Certificates.Admin;

namespace TEC.FullService.Api.Domain;

/// <summary>
///     Repræsenterer et certifikat, en bruger har opnået.
///     Kan oprettes automatisk efter en gennemført tilmelding eller manuelt.
/// </summary>
public class Certificate : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public string CertificateName { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public CertificateStatus Status { get; set; }

    // Fremmednøgler
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public Guid? EnrollmentId { get; set; } // Nullable, for manuelt oprettede certifikater
    public Guid? ReplacesId { get; set; } // Nullable, for fornyelseshistorik
    public Guid? CompetenceFundId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public Enrollment? Enrollment { get; set; } // behøver vi overhovedet denne?
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
    /// <param name="req"></param>
    public void ApplyPatch(PatchCertificateRequest req)
    {
        if (req.CertificateName is not null)
        {
            ValidateName(req.CertificateName);
            CertificateName = req.CertificateName;
        }

        if (req.IssueDate.HasValue)
        {
            // Hvis vi opdaterer IssueDate, skal vi validere ift. enten ny ExpiryDate eller eksisterende.
            var newIssue = req.IssueDate.Value;
            var newExpiry = req.ExpiryDate ?? ExpiryDate;
            ValidateDates(newIssue, newExpiry);
            IssueDate = newIssue;
        }

        if (req.ExpiryDate.HasValue)
        {
            var newIssue = req.IssueDate ?? IssueDate;
            var newExpiry = req.ExpiryDate.Value;
            ValidateDates(newIssue, newExpiry);
            ExpiryDate = newExpiry;
        }

        if (req.Status.HasValue)
            Status = req.Status.Value;

        if (req.UserId.HasValue)
            UserId = req.UserId.Value;

        if (req.CourseId.HasValue)
            CourseId = req.CourseId.Value;

        if (req.EnrollmentId.HasValue)
            EnrollmentId = req.EnrollmentId.Value;

        if (req.ReplacesId.HasValue)
        {
            ValidateReplacesId(req.ReplacesId.Value);
            ReplacesId = req.ReplacesId.Value;
        }

        if (req.CompetenceFundId.HasValue)
            CompetenceFundId = req.CompetenceFundId.Value;
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
}
