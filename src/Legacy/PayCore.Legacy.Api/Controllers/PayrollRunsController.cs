using Microsoft.AspNetCore.Mvc;
using PayCore.Legacy.Api.Dtos;
using PayCore.Legacy.Api.Services;

namespace PayCore.Legacy.Api.Controllers;

[ApiController]
[Route("api/payroll-runs")]
public class PayrollRunsController : ControllerBase
{
    private readonly LegacyPayrollService _service;

    public PayrollRunsController(LegacyPayrollService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var runs = await _service.GetPayrollRunsAsync();
        return Ok(runs);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetPayrollRunWithDetailsAsync(id);
        if (result is null)
            return NotFound(new { message = "Not found" });

        return Ok(new { run = result.Value.Run, details = result.Value.Details });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePayrollRunRequest request)
    {
        if (request is null)
            return BadRequest(new { error = "Request body is required." });

        var created = await _service.CreatePayrollRunAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.PayrollRunId }, created);
    }
}
