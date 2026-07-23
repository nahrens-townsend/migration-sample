# ADR-003 · Centralized NuGet Package Management

**Date**: 2026-07-23  
**Status**: Accepted

---

## Context

The solution contains 13 projects. Without central version control, each `.csproj` independently
pins package versions. In practice this leads to:

- Different projects pulling different patch versions of the same package (version drift).
- Security vulnerability fixes applied inconsistently — some projects patched, others not.
- `dotnet add package` upgrading one project silently while others lag behind.
- PR diffs cluttered with version bumps scattered across many `.csproj` files.

## Decision

All NuGet package versions are declared once in `Directory.Packages.props` at the repository root
using [NuGet Central Package Management (CPM)](https://aka.ms/nuget/cpm/gettingstarted):

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="SomePackage" Version="x.y.z" />
  </ItemGroup>
</Project>
```

Individual `.csproj` files declare `<PackageReference Include="SomePackage" />` **without** a
`Version` attribute. The version is resolved centrally.

### How to add a new package

1. Add a `<PackageVersion>` entry to `Directory.Packages.props`.
2. Add a version-less `<PackageReference>` to the consuming `.csproj`.
3. Run `dotnet restore`.

### Overriding transitive dependency versions

When a transitive dependency has a known vulnerability (e.g., `Microsoft.OpenApi` < 2.7.5),
pin the patched version in `Directory.Packages.props` **and** add it as a direct
`<PackageReference>` in the affected project. CPM `PackageVersion` entries alone do not
override transitive resolution.

## Consequences

### Positive

- One-line PR to upgrade a package across all 13 projects.
- Security patches applied uniformly — no project left behind.
- Dependency audit (`dotnet list package --vulnerable`) is accurate across the whole solution.

### Negative / Trade-offs

- `dotnet new` and `dotnet add package` templates inject `Version=` attributes that must be removed
  after scaffolding — a manual but mechanical step.
- Projects cannot independently pin a different version of a shared package without a `VersionOverride`
  attribute (use sparingly; document the reason inline).

### Neutral

- `Directory.Build.props` centralizes MSBuild properties (`Nullable`, `ImplicitUsings`,
  `TreatWarningsAsErrors`, `TargetFramework`) using the same single-source principle.
