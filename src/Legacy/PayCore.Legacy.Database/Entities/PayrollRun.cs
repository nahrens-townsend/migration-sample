namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// A single payroll run. Intentional flaws:
/// - PeriodEndDate / PeriodStartDate stored as NVARCHAR(20) strings, not DateTime
/// - No FK to a period-definition table; period is inferred from the string dates
/// - No CreatedAt / UpdatedAt
/// </summary>
public class PayrollRun
{
    public int PayrollRunId { get; set; }

    public DateTime RunDate { get; set; }

    /// <summary>Legacy: stored as NVARCHAR(20) e.g. "2026-07-15".</summary>
    public string PeriodEndDate { get; set; } = string.Empty;

    /// <summary>Legacy: stored as NVARCHAR(20).</summary>
    public string PeriodStartDate { get; set; } = string.Empty;

    /// <summary>Free-text status: "OPEN", "PROCESSED", "PAID", "REVERSED".</summary>
    public string Status { get; set; } = string.Empty;

    public decimal TotalGross { get; set; }

    public decimal TotalNet { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal TotalTax { get; set; }

    public string ProcessedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
