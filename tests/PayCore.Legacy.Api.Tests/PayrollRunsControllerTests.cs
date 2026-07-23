using System.Net;

namespace PayCore.Legacy.Api.Tests;

public sealed class PayrollRunsControllerTests
{
    private const string ApiKey = "dev-api-key-change-me";

    [Fact]
    public async Task GetPayrollRuns_Returns200()
    {
        using var factory = new TestApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await client.GetAsync("/api/payroll-runs");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("[", body.TrimStart());
    }
}
