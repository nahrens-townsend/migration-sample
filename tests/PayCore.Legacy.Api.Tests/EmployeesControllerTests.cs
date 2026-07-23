using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace PayCore.Legacy.Api.Tests;

public sealed class EmployeesControllerTests
{
    private const string ApiKey = "dev-api-key-change-me";

    [Fact]
    public async Task GetEmployees_Returns200_WithCorrectShape()
    {
        using var factory = new TestApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await client.GetAsync("/api/employees");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("[", body.TrimStart());
    }

    [Fact]
    public async Task PostEmployee_ValidBody_Returns201()
    {
        using var factory = new TestApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var payload = new
        {
            employeeNumber = "E001",
            firstName = "Test",
            lastName = "User",
            socialInsuranceNumber = "1234567890",
            hireDate = "2026-01-01",
            province = "ON"
        };

        var response = await client.PostAsJsonAsync("/api/employees", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("employeeId", body);
    }

    [Fact]
    public async Task PostEmployee_MissingRequiredField_Returns400()
    {
        using var factory = new TestApiFactory();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await client.PostAsync(
            "/api/employees",
            new StringContent("", Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
