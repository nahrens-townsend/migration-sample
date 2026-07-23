using Microsoft.AspNetCore.Mvc;
using PayCore.Legacy.Api.Services;

namespace PayCore.Legacy.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly LegacyPayrollService _service;

    public ReportsController(LegacyPayrollService service)
    {
        _service = service;
    }

    [HttpGet("ytd-summary")]
    public async Task<IActionResult> YtdSummary([FromQuery] int year = 0)
    {
        if (year == 0)
            year = DateTime.UtcNow.Year;

        var summary = await _service.GetYtdSummaryAsync(year);
        return Ok(summary);
    }
}
