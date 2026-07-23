using Microsoft.EntityFrameworkCore;
using PayCore.Legacy.Api.Middleware;
using PayCore.Legacy.Api.Services;
using PayCore.Legacy.Database.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LegacyPayrollContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LegacyDb")));

builder.Services.AddScoped<LegacyEmployeeService>();
builder.Services.AddScoped<LegacyPayrollService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v0", new() { Title = "PayCore Legacy API", Version = "v0" });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v0/swagger.json", "PayCore Legacy API v0"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
