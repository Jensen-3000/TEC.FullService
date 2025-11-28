# FullService.Infrastructure

This project contains the infrastructure layer for the FullService system.  
It provides database access, Entity Framework Core contexts, and ASP.NET Identity integration.  
No business rules or domain logic live here — only technical concerns.

---

## Project Overview

**Identity/**
- `ApplicationUser.cs`  
  Custom user model based on ASP.NET IdentityUser.

- `IdentityDbContext.cs`  
  Stores authentication and authorization data (users, roles, claims).

**Data/**
- `FullServiceDbContext.cs`  
  Stores application domain data such as Companies, Certificates, etc.

**Configuration/**
- `DatabaseSettings.cs`  
  Strongly-typed connection string settings used by both DbContexts.

**Extensions/**
- `InfrastructureExtensions.cs`  
  Registers database contexts and the Identity subsystem with the DI container.

---

## Configuration

During development, connection strings are supplied through  
`appsettings.Development.json` in the API project:

```
"ConnectionStrings": {
  "IdentityConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FullService_Identity_Dev;Trusted_Connection=True;TrustServerCertificate=True;",
  "FullServiceConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FullService_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

These values are bound to `DatabaseSettings` on startup.  
Production/staging environments should override these using environment variables or secret stores.

---

## Working with EF Core Migrations

This solution uses two separate EF Core DbContexts:

1. `IdentityDbContext`
2. `FullServiceDbContext`

Each context requires its own migration commands.

### Create Identity migrations

```
Add-Migration InitialIdentity `
  -Context IdentityDbContext `
  -Project TEC.FullService.Infrastructure `
  -StartupProject TEC.FullService.Api `
  -OutputDir MigrationsIdentity
```

### Apply Identity migrations

```
Update-Database `
  -Context IdentityDbContext `
  -Project TEC.FullService.Infrastructure `
  -StartupProject TEC.FullService.Api
```

### Create FullService (domain) migrations

```
Add-Migration InitialFullService `
  -Context FullServiceDbContext `
  -Project TEC.FullService.Infrastructure `
  -StartupProject TEC.FullService.Api `
  -OutputDir MigrationsFullService
```

### Apply FullService migrations

```
Update-Database `
  -Context FullServiceDbContext `
  -Project TEC.FullService.Infrastructure `
  -StartupProject TEC.FullService.Api
```

---

## Notes

- Infrastructure depends only on the Domain layer.  
- Migrations are committed to source control.  
- Local development uses LocalDB; production should use a managed SQL Server instance.  
- The API project is responsible for binding runtime configuration, including database settings.  
