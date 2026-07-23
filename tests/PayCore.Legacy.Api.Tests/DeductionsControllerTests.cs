using System.Net;
using Microsoft.Extensions.DependencyInjection;
using PayCore.Legacy.Database.Context;

namespace PayCore.Legacy.Api.Tests;

public sealed class DeductionsControllerTests
{
    private const string ApiKey = "dev-api-key-change-me";

    [Fact]
    public async Task GetDeductions_Returns200()
    {
        using var factory = new TestApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await client.GetAsync("/api/deductions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("[", body.TrimStart());
    }
}
