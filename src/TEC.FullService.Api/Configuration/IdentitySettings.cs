using System.ComponentModel.DataAnnotations;

namespace TEC.FullService.Api.Configuration;

public sealed class IdentitySettings
{
    [Range(8, 128)]
    public int PasswordRequiredLength { get; init; } = 8;
    public bool RequireUniqueEmail { get; init; } = true;
    public bool PasswordRequireLowercase { get; init; } = true;
    public bool PasswordRequireUppercase { get; init; } = true;
    public bool PasswordRequireNonAlphanumeric { get; init; } = true;
}
