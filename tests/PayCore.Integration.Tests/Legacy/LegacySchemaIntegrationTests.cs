using Microsoft.EntityFrameworkCore;
using PayCore.Legacy.Database.Context;

namespace PayCore.Integration.Tests.Legacy;

/// <summary>
/// Integration tests that verify the legacy EF Core migration applies cleanly
/// and produces the expected schema in a real SQL Server database.
///
/// Each test run gets an isolated database (<c>PayCore_Legacy_Test_{Guid}</c>)
/// that is dropped in <see cref="DisposeAsync"/> — no shared state between runs.
/// </summary>
public sealed class LegacySchemaIntegrationTests : IAsyncLifetime
{
    // Expected legacy table names — must match OnModelCreating ToTable() calls exactly.
    private static readonly IReadOnlyList<string> ExpectedTables =
    [
        "EMPLOYEE_MASTER",
        "PAYROLL_RUN",
        "PAYROLL_RUN_DETAIL",
        "DEDUCTION_CODE",
        "BENEFIT_ENROLLMENT",
        "TAX_TABLE",
        "COMPANY_SETTINGS",
        "AUDIT_LOG",
    ];

    private LegacyPayrollContext _context = null!;

    // ── IAsyncLifetime ───────────────────────────────────────────────────────

    public async Task InitializeAsync()
    {
        var connectionString = BuildConnectionString();
        var options = new DbContextOptionsBuilder<LegacyPayrollContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new LegacyPayrollContext(options);
        await _context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    // ── Tests ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task MigrateAsync_CreatesAllEightLegacyTables()
    {
        // Query the real schema — not EF metadata — so we confirm the DDL landed.
        var actualTables = await _context.Database
            .SqlQueryRaw<string>(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME != '__EFMigrationsHistory'")
            .ToListAsync();

        foreach (var expected in ExpectedTables)
        {
            Assert.Contains(expected, actualTables);
        }

        Assert.Equal(ExpectedTables.Count, actualTables.Count);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Builds a per-run connection string with a unique database name.
    /// Uses <c>LEGACY_DB_CONNECTION</c> as the server template when set (CI),
    /// otherwise targets LocalDB.
    /// </summary>
    private static string BuildConnectionString()
    {
        var dbName = $"PayCore_Legacy_Test_{Guid.NewGuid():N}";

        var envValue = Environment.GetEnvironmentVariable("LEGACY_DB_CONNECTION");
        if (!string.IsNullOrWhiteSpace(envValue))
        {
            // Replace the Database segment from the CI connection string template.
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(envValue)
            {
                InitialCatalog = dbName
            };
            return builder.ConnectionString;
        }

        return $"Server=(localdb)\\MSSQLLocalDB;Database={dbName};Trusted_Connection=True;";
    }
}
