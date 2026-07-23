# ADR-005 · Legacy Schema Snapshot CI Enforcement

**Date**: 2026-07-23  
**Status**: Accepted

---

## Context

After the `LegacyPayrollContext` and its initial migration were created in Phase 2, a developer could
accidentally modify an entity class or `OnModelCreating` configuration without running
`dotnet ef migrations add` to record that change. The migration snapshot would then be out of sync
with the model, causing silent data-loss or schema drift when `dotnet ef database update` runs later.

A secondary concern is test isolation: the existing CI job runs on `ubuntu-latest`, which does not
have LocalDB. Integration tests that call `MigrateAsync()` against a real SQL Server database
cannot run without a database service.

## Decision

1. **Schema snapshot check**: Add a CI step that runs  
   `dotnet ef migrations has-pending-model-changes` after `Build` and before `Test`.  
   The command exits non-zero if the current EF model differs from the most recent migration's
   model snapshot, failing the CI build before any test runs.

2. **SQL Server service container**: Add a `mcr.microsoft.com/mssql/server:2022-latest` Docker
   service to the `build-and-test` job. Integration tests receive the connection string via the
   `LEGACY_DB_CONNECTION` environment variable.  
   LocalDB is **not** used in CI because it is Windows-only and CI runs `ubuntu-latest`.

3. **EF Core global tool**: Install `dotnet-ef` as a global tool in CI (pinned to `10.0.10`)
   because it is not guaranteed to be pre-installed on GitHub-hosted runners.

## Consequences

### Positive

- Developers who forget to run `dotnet ef migrations add` after a model change will see an
  immediate CI failure with a clear error message — before the unrecorded change reaches `main`.
- Integration tests run against a real SQL Server engine in CI, matching the production database
  engine and catching SQL Server-specific issues (collation, type coercion, etc.) that in-memory
  providers would miss.

### Negative / Trade-offs

- The SQL Server container adds ~30–60 s of startup time to the CI job (health-check polling).
- The SA password (`YourStrong!Passw0rd`) is committed in plain text in `ci.yml`. This is
  acceptable for an ephemeral, network-isolated Docker container; the password grants access only
  to the throwaway test database and is not reused anywhere else.
- Developers running integration tests locally still need either LocalDB (Windows) or a local
  SQL Server Docker container; the `LEGACY_DB_CONNECTION` env var must be set when not using
  LocalDB.

### Neutral

- `dotnet ef migrations has-pending-model-changes` does **not** connect to a database; it compares
  the in-memory EF model against the compiled model snapshot (`LegacyPayrollContextModelSnapshot.cs`).
  The SQL Server service container is therefore not needed for the snapshot check step.
