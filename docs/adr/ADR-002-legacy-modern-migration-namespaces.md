# ADR-002 · Legacy / Modern / Migration Namespace Isolation

**Date**: 2026-07-23  
**Status**: Accepted

---

## Context

During the migration period two independent stacks coexist in the same repository:

| Namespace prefix    | Projects                                              | Purpose                                 |
| ------------------- | ----------------------------------------------------- | --------------------------------------- |
| `PayCore.Legacy.*`  | `PayCore.Legacy.Database`, `PayCore.Legacy.Api`       | Read-only façade over the existing DB   |
| `PayCore.*`         | Domain, Application, Infrastructure, Api, Workers     | New target architecture                 |
| `PayCore.Migration.*` | Migration.Core, Migration.DataAccess, Migration.Runner | One-way data migration tooling        |

Allowing arbitrary cross-namespace project references during the migration introduces tight coupling
between the old and new systems, making it impossible to retire the legacy stack cleanly.

## Decision

Cross-namespace project references are **prohibited** during the migration period:

- `PayCore.Legacy.*` projects must not reference `PayCore.*` or `PayCore.Migration.*` projects.
- `PayCore.*` projects must not reference `PayCore.Legacy.*` projects.
- `PayCore.Migration.*` projects may reference both stacks (they exist to bridge them), but only
  `PayCore.Migration.DataAccess` may do so; `PayCore.Migration.Core` is reference-free.

This rule is enforced via `.csproj` project-reference constraints and validated in CI.

## Consequences

### Positive

- The legacy system can be deleted (`.csproj` removed from solution) without breaking the modern stack.
- The migration tooling is the only component that knows about both namespaces — blast radius is contained.
- Clear ownership: changes to `PayCore.Legacy.*` cannot accidentally affect production modern endpoints.

### Negative / Trade-offs

- Shared models (e.g., money types, enums) must be duplicated or mapped explicitly at the migration
  boundary rather than shared via a common project.
- Some duplication of infrastructure code (e.g., connection string configuration) across stacks.

### Neutral

- Once migration is complete and `PayCore.Legacy.*` is retired, this ADR is superseded by ADR-001 alone.
