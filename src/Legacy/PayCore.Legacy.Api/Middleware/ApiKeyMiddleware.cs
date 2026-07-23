namespace PayCore.Legacy.Api.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate    _next;
    private readonly IConfiguration     _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next          = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var expectedKey = _configuration["ApiKey"];

        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var providedKey)
            || providedKey != expectedKey)
        {
            context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
            return;
        }

        await _next(context);
    }
}
