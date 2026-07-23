using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PayCore.Legacy.Database.Context;

public class LegacyDatabaseDesignTimeFactory : IDesignTimeDbContextFactory<LegacyPayrollContext>
{
    public LegacyPayrollContext CreateDbContext(string[] args)
    {
        // EF Core design-time tooling sets CWD to the project directory,
        // so appsettings.json is found relative to the project root.
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        // appsettings.json is authoritative; LEGACY_DB_CONNECTION is the CI fallback.
        // Treat null, empty, and whitespace-only values as absent so the fallback chain works.
        var fromJson = configuration.GetConnectionString("LegacyPayroll");
        var fromEnv  = Environment.GetEnvironmentVariable("LEGACY_DB_CONNECTION");

        var connectionString = !string.IsNullOrWhiteSpace(fromJson) ? fromJson
            : !string.IsNullOrWhiteSpace(fromEnv)  ? fromEnv
            : throw new InvalidOperationException(
                "No connection string found. Provide 'ConnectionStrings:LegacyPayroll' in " +
                "appsettings.json or set the LEGACY_DB_CONNECTION environment variable.");

        var optionsBuilder = new DbContextOptionsBuilder<LegacyPayrollContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LegacyPayrollContext(optionsBuilder.Options);
    }
}
