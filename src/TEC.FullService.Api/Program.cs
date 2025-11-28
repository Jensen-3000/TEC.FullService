using FastEndpoints;
using TEC.FullService.Infrastructure.Configuration;
using TEC.FullService.Infrastructure.Data;
using TEC.FullService.Infrastructure.Extensions;
using TEC.FullService.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Bind database settings
builder.Services
    .AddOptions<DatabaseSettings>()
    .Bind(builder.Configuration.GetSection(DatabaseSettings.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Setup infrastructure services
builder.Services.AddInfrastructure();

builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Ensure databases are created in development environment
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await identityDb.Database.EnsureCreatedAsync();

    var fullDb = scope.ServiceProvider.GetRequiredService<FullServiceDbContext>();
    await fullDb.Database.EnsureCreatedAsync();
}

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();

