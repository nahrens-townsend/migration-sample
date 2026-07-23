using Microsoft.AspNetCore.Mvc;
using PayCore.Legacy.Api.Services;

namespace PayCore.Legacy.Api.Controllers;

[ApiController]
[Route("api/deductions")]
public class DeductionsController : ControllerBase
{
    private readonly LegacyPayrollService _service;

    public DeductionsController(LegacyPayrollService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var codes = await _service.GetDeductionCodesAsync();
        return Ok(codes);
    }
}
