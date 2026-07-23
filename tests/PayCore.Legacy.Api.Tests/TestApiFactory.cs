using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayCore.Legacy.Database.Context;

namespace PayCore.Legacy.Api.Tests;

internal sealed class TestApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // ConfigureTestServices runs AFTER Program.cs registers services.
        builder.ConfigureTestServices(services =>
        {
            // Remove the SQL-Server-wired scoped registration.
            foreach (var d in services.Where(s => s.ServiceType == typeof(LegacyPayrollContext)).ToList())
                services.Remove(d);

            // Build InMemory options with their OWN isolated EF internal service provider
            // so they do not conflict with the SQL Server EF services already in the container.
            var dbOptions = new DbContextOptionsBuilder<LegacyPayrollContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
                .Options;

            // Register the context directly — no second AddDbContext call, which would add
            // a second EF provider to the main DI container and trigger the "two providers" error.
            services.AddScoped<LegacyPayrollContext>(_ => new LegacyPayrollContext(dbOptions));
        });

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiKey"] = "dev-api-key-change-me"
            });
        });
    }
}
