# PayCore Migration Sample

End-to-end demonstration of migrating a legacy payment-processing system to a modern .NET 10 architecture.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) ≥ 10.0.201
- SQL Server (local or Docker)

## Quick start

```bash
dotnet build
dotnet test
```

## Solution map

```
src/
  Legacy/
    PayCore.Legacy.Database/   – Legacy schema (EF Core scaffolded models)
    PayCore.Legacy.Api/        – Read-only façade over the legacy DB
  Modern/
    PayCore.Domain/            – Core domain entities & value objects
    PayCore.Application/       – Use cases, CQRS commands/queries
    PayCore.Infrastructure/    – EF Core DbContext, repositories, external services
    PayCore.Api/               – ASP.NET Core Minimal API host
    PayCore.BackgroundWorkers/ – Hosted services for async processing
  Migration/
    PayCore.Migration.Core/        – Migration abstractions & domain logic
    PayCore.Migration.DataAccess/  – Data readers/writers for both databases
    PayCore.Migration.Runner/      – CLI orchestrator
tests/                         – xUnit test projects
docs/adr/                      – Architecture Decision Records
scripts/                       – Utility scripts
.github/workflows/             – CI/CD pipelines
```

## Build properties (applied globally)

| Property               | Value      |
| ---------------------- | ---------- |
| `TargetFramework`      | net10.0    |
| `Nullable`             | enable     |
| `ImplicitUsings`       | enable     |
| `TreatWarningsAsErrors`| true       |

Package versions are centrally managed in `Directory.Packages.props`.
