using System.ComponentModel.DataAnnotations;

namespace TEC.FullService.Api.Configuration;

public sealed class DatabaseSettings
{
    [Required, MinLength(3)]
    public string IdentityConnectionString { get; init; } = string.Empty;

    [Required, MinLength(3)]
    public string FullServiceConnectionString { get; init; } = string.Empty;
}
