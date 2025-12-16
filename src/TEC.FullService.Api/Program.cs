using System.Diagnostics;
using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using TEC.FullService.Api.Common;
using TEC.FullService.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier);

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

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

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Errors.UseProblemDetails();
    c.Errors.ProducesMetadataType = typeof(ProblemDetails);
    c.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    c.Endpoints.RoutePrefix = "api";

#if DEBUG
    c.Endpoints.Configurator = ep => ep.AllowAnonymous();
#endif

})
.UseSwaggerGen();

app.Run();
