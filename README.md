# FullService – Quick Start

This guide explains how to start the FullService solution locally with minimal steps.  
It covers the essential tools, configuration, and workflow needed to begin development immediately.

---

## 1. Requirements

Install the following tools before you start:

- .NET 10 SDK  
- Visual Studio 2026 or newer  
- SQL Server Express / LocalDB  
- Git 2.40+  

Clone the repository and open the solution in Visual Studio.

---

## 2. Solution Structure (Overview)

The solution contains these core projects:

- **TEC.FullService.Api**  
  Hosts the backend Web API, Identity, authentication, and FastEndpoints.

- **TEC.FullService.Web**  
  Blazor Web App UI using server-side interactivity (Interactive Server render mode).

- **TEC.FullService.Infrastructure**  
  Data access, EF Core DbContexts, Identity setup, connection strings, and persistence code.

- **TEC.FullService.Application**  
  Vertical slices (handlers, validators, requests, responses).

- **TEC.FullService.Domain**  
  Entities, value objects, and domain logic.

Infrastructure depends on Domain.  
API and Web depend on Application + Infrastructure.

---

## 3. Local Database Setup (Development Only)

The system uses two separate databases in development:

1. Identity database  
2. Main application database  

Connection strings are defined in:

`TEC.FullService.Api/appsettings.Development.json`

Example (LocalDB):

```
"ConnectionStrings": {
  "IdentityConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FullService_Identity_Dev;Trusted_Connection=True;TrustServerCertificate=True;",
  "FullServiceConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FullService_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

When the API runs in Development mode, both databases are created automatically using `EnsureCreated()`.  
No manual EF Core commands are needed for initial startup.

---

## 4. Running the Backend (API)

Start the API project:

- HTTPS endpoint:  
  `https://localhost:5001`

Swagger UI will open automatically.

From command line:

```
dotnet run --project TEC.FullService.Api
```

---

## 5. Running the Frontend (Blazor Server)

Start the Web project:

- HTTPS endpoint:  
  `https://localhost:7001`

From command line:

```
dotnet run --project TEC.FullService.Web
```

---

## 6. Running Both Together

The solution is preconfigured to start both the API and Web projects simultaneously.
If not defaulted, you can select "FullService.Dev" to launch both projects.

Press **F5** in Visual Studio to launch:

- TEC.FullService.Api (https://localhost:5001)
- TEC.FullService.Web (https://localhost:7001)

Swagger opens automatically for the API, and the Blazor Web App UI opens for the frontend.

No additional configuration is required for developers.

---

## 7. Typical Development Workflow

1. Modify or add domain entities  
2. Add vertical slices in the Application project (Request/Handler/Validator)  
3. Implement endpoints in the API using FastEndpoints  
4. Run API + Web together  
5. Test using Swagger and Blazor UI  

Migrations are handled separately and are not required for initial development.  
TODO: Add extended guide -- See the extended developer guide in `/docs/DeveloperGuide.md` for details.

---

## 8. Notes

- HTTPS is mandatory for all development and production endpoints.  
- Identity and application databases are created automatically in Development.  
- Infrastructure contains data access only; keep business logic inside Domain.  
- All source code must be committed via Git.  

---

## Commit Message Standard for AI-Generated Commits

### Visual Studio 2026 Integration
To enable Copilot repository instructions:

Tools → Options → GitHub Copilot → Source Control Integration

Once enabled, use "Generate Git commit message" in the Git Changes window.

### AI Commit Format (Conventional Commits v1.0.0)
Add the following configuration to Visual Studio 2026 to enforce the format:

```text
Use Conventional Commits v1.0.0.
Format: <type>(optional scope): short imperative description.
Allowed types: feat, fix, docs, style, refactor, perf, test, chore, ci.
One single line only.
No commit body.
No explanations.
No file lists.
No bullet points.
Subject max ~50 characters, imperative English.
