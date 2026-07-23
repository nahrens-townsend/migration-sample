# ADR-004 · EF Core Migrations Per Project Boundary

**Date**: 2026-07-23  
**Status**: Accepted

---

## Context

Two independent EF Core DbContexts exist in this solution:

| Project                      | DbContext             | Database          |
| ---------------------------- | --------------------- | ----------------- |
| `PayCore.Legacy.Database`    | `LegacyDbContext`     | Legacy SQL Server |
| `PayCore.Infrastructure`     | `PayCoreDbContext`    | Modern SQL Server |

During migration, both databases exist simultaneously. A shared or merged migration history would
couple the two schema evolution tracks, making it impossible to apply a legacy schema fix without
also touching the modern schema (and vice versa).

## Decision

Each project owns an independent, self-contained EF Core migration history:

- `PayCore.Legacy.Database` — migrations targeting the legacy schema, run via
  `dotnet ef migrations add` with `--project src/Legacy/PayCore.Legacy.Database`.
- `PayCore.Infrastructure` — migrations targeting the modern schema, run via
  `dotnet ef migrations add` with `--project src/Modern/PayCore.Infrastructure`.

A shared `DbContext` that spans both databases is **prohibited**. The migration runner
(`PayCore.Migration.Runner`) reads from one DbContext and writes to another; it never owns
a migration history itself.

### Naming convention

- Legacy migrations: `Legacy_YYYYMMDD_NNN_Description`
- Modern migrations: `YYYYMMDD_NNN_Description`

This prevents accidental filename collisions if both projects are ever in the same directory.

## Consequences

### Positive

- Legacy schema changes (e.g., adding an index for read performance) are applied without touching
  modern migration history.
- Modern schema can evolve independently once the migration is complete.
- `PayCore.Legacy.Database` can be deleted (along with its migration history) post-migration without
  affecting `PayCore.Infrastructure` migrations.

### Negative / Trade-offs

- Two separate `dotnet ef database update` commands must be run when both schemas change.
- Developers must remember which project owns which context; tooling defaults to the project in the
  current directory, which may be surprising.

### Neutral

- EF Core Tools (`Microsoft.EntityFrameworkCore.Tools`) is declared with `<PrivateAssets>all</PrivateAssets>`
  in both projects — it is a build-time tool only and must not appear in published output.
