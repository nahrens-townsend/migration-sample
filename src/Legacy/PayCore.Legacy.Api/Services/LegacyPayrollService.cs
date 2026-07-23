using Microsoft.EntityFrameworkCore;
using PayCore.Legacy.Api.Dtos;
using PayCore.Legacy.Database.Context;
using PayCore.Legacy.Database.Entities;

namespace PayCore.Legacy.Api.Services;

public class LegacyPayrollService
{
    private readonly LegacyPayrollContext _db;

    public LegacyPayrollService(LegacyPayrollContext db)
    {
        _db = db;
    }

    public async Task<List<PayrollRunDto>> GetPayrollRunsAsync()
    {
        var runs = await _db.PayrollRuns.OrderByDescending(r => r.RunDate).ToListAsync();
        return runs.Select(MapRunToDto).ToList();
    }

    public async Task<(PayrollRunDto Run, List<PayrollRunDetailDto> Details)?> GetPayrollRunWithDetailsAsync(int id)
    {
        var run = await _db.PayrollRuns.FindAsync(id);
        if (run is null) return null;

        var details = await _db.PayrollRunDetails
            .Where(d => d.PayrollRunId == id)
            .ToListAsync();

        return (MapRunToDto(run), details.Select(MapDetailToDto).ToList());
    }

    public async Task<PayrollRunDto> CreatePayrollRunAsync(CreatePayrollRunRequest request)
    {
        var run = new PayrollRun
        {
            RunDate         = DateTime.UtcNow,
            PeriodStartDate = request.PeriodStartDate,
            PeriodEndDate   = request.PeriodEndDate,
            ProcessedBy     = request.ProcessedBy,
            Notes           = request.Notes,
            Status          = "OPEN",
            TotalGross      = 0,
            TotalNet        = 0,
            TotalDeductions = 0,
            TotalTax        = 0
        };

        _db.PayrollRuns.Add(run);
        await _db.SaveChangesAsync();

        return MapRunToDto(run);
    }

    public async Task<List<DeductionCodeDto>> GetDeductionCodesAsync()
    {
        var codes = await _db.DeductionCodes.OrderBy(c => c.Code).ToListAsync();
        return codes.Select(MapDeductionCodeToDto).ToList();
    }

    public async Task<List<YtdSummaryDto>> GetYtdSummaryAsync(int year)
    {
        var yearStr = year.ToString();

        var runIds = await _db.PayrollRuns
            .Where(r => r.PeriodStartDate.StartsWith(yearStr))
            .Select(r => r.PayrollRunId)
            .ToListAsync();

        var details = await _db.PayrollRunDetails
            .Where(d => runIds.Contains(d.PayrollRunId))
            .ToListAsync();

        var employeeIds = details.Select(d => d.EmployeeId).Distinct().ToList();

        var employees = await _db.Employees
            .Where(e => employeeIds.Contains(e.EmployeeId))
            .ToListAsync();

        var result = details
            .GroupBy(d => d.EmployeeId)
            .Select(g =>
            {
                var emp            = employees.FirstOrDefault(e => e.EmployeeId == g.Key);
                var totalGross      = g.Sum(d => d.Amount);
                var totalDeductions = g.Sum(d => d.DeductionAmount ?? 0m);
                var totalTax        = g.Sum(d => d.TaxAmount ?? 0m);

                return new YtdSummaryDto
                {
                    EmployeeId      = g.Key,
                    EmployeeNumber  = emp?.EmployeeNumber ?? string.Empty,
                    FullName        = emp is not null ? $"{emp.FirstName} {emp.LastName}" : string.Empty,
                    TotalGross      = totalGross,
                    TotalDeductions = totalDeductions,
                    TotalTax        = totalTax,
                    TotalNet        = totalGross - totalDeductions - totalTax
                };
            })
            .OrderBy(s => s.EmployeeNumber)
            .ToList();

        return result;
    }

    private static PayrollRunDto MapRunToDto(PayrollRun r) => new()
    {
        PayrollRunId    = r.PayrollRunId,
        RunDate         = r.RunDate,
        PeriodStartDate = r.PeriodStartDate,
        PeriodEndDate   = r.PeriodEndDate,
        Status          = r.Status,
        TotalGross      = r.TotalGross,
        TotalNet        = r.TotalNet,
        TotalDeductions = r.TotalDeductions,
        TotalTax        = r.TotalTax,
        ProcessedBy     = r.ProcessedBy,
        Notes           = r.Notes
    };

    private static PayrollRunDetailDto MapDetailToDto(PayrollRunDetail d) => new()
    {
        DetailId        = d.DetailId,
        PayrollRunId    = d.PayrollRunId,
        EmployeeId      = d.EmployeeId,
        EarningsCode    = d.EarningsCode,
        Description     = d.Description,
        Amount          = d.Amount,
        Hours           = d.Hours,
        TaxAmount       = d.TaxAmount,
        DeductionAmount = d.DeductionAmount,
        OverrideNote    = d.OverrideNote
    };

    private static DeductionCodeDto MapDeductionCodeToDto(DeductionCode c) => new()
    {
        DeductionCodeId = c.DeductionCodeId,
        Code            = c.Code,
        Description     = c.Description,
        DeductionType   = c.DeductionType,
        IsPercentage    = c.IsPercentage,
        Amount          = c.Amount,
        Percentage      = c.Percentage,
        IsActive        = c.IsActive
    };
}
