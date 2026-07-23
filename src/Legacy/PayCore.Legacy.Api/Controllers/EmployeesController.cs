using Microsoft.AspNetCore.Mvc;
using PayCore.Legacy.Api.Dtos;
using PayCore.Legacy.Api.Services;

namespace PayCore.Legacy.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly LegacyEmployeeService _service;

    public EmployeesController(LegacyEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var employees = await _service.GetEmployeesAsync(page, pageSize);
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _service.GetEmployeeByIdAsync(id);
        if (employee is null)
            return NotFound(new { error = "Not found" });

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        if (request is null)
            return BadRequest(new { message = "Request body is required." });

        var created = await _service.CreateEmployeeAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        var updated = await _service.UpdateEmployeeAsync(id, request);
        if (updated is null)
            return NotFound(new { error = "Not found" });

        return Ok(updated);
    }
}
