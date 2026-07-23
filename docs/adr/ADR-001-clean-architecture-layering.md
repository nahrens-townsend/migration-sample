# ADR-001 · Clean Architecture Layering

**Date**: 2026-07-23  
**Status**: Accepted

---

## Context

The modern stack (`PayCore.*`) replaces a legacy monolith. Without enforced boundaries, business logic
leaks into infrastructure code and UI code accumulates database queries — the same pattern that made
the legacy codebase expensive to change.

We need a dependency rule that is checked mechanically (compile-time project references) rather than
by convention alone.

## Decision

We adopt a strict inward-only dependency rule across four layers:

```
PayCore.Api  ──►  PayCore.Application  ──►  PayCore.Domain
     │                                             ▲
     └──►  PayCore.Infrastructure  ───────────────┘
```

- **Domain** (`PayCore.Domain`) — zero external NuGet dependencies; no references to any other project.
  Contains entities, value objects, domain events, and repository interfaces.
- **Application** (`PayCore.Application`) — references Domain only.
  Contains use cases (commands/queries), application services, and DTOs.
- **Infrastructure** (`PayCore.Infrastructure`) — references Application (and transitively Domain).
  Contains EF Core DbContext, repository implementations, and external service adapters.
- **Api** (`PayCore.Api`) — references Application **and** Infrastructure.
  Infrastructure is referenced here **only** for dependency-injection composition in `Program.cs`.
  No Infrastructure types may appear in controllers or endpoint handlers.

Any project reference that would create an outward dependency (e.g., Application → Infrastructure)
is prohibited at the `.csproj` level and will fail to compile.

## Consequences

### Positive

- Business rules in Domain are pure C# — fast to test, no mocking of EF Core.
- Swapping persistence (e.g., SQL Server → PostgreSQL) touches only Infrastructure.
- New application use cases can be written and unit-tested without a database.

### Negative / Trade-offs

- Developers must understand the layer model before contributing; initial onboarding cost.
- Cross-cutting concerns (logging, validation) require abstractions defined in Application or Domain
  rather than direct use of infrastructure libraries.

### Neutral

- `PayCore.BackgroundWorkers` follows the same rule: references Application only;
  any persistence is injected via Application interfaces.
