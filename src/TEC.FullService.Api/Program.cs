using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using TEC.FullService.Api.Extensions;
using TEC.FullService.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddFastEndpoints();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();

    var identityDb = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
    var fullDb = scope.ServiceProvider.GetRequiredService<FullServiceDbContext>();

    await identityDb.Database.EnsureCreatedAsync();
    await fullDb.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();
