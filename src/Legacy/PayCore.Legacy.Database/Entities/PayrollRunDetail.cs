namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Line-item detail for a payroll run. Intentional flaws:
/// - PayrollRunId and EmployeeId have no database FK constraints (orphaned rows are possible)
/// - EarningsCode is a free-text string, not an FK to a code table
/// - Mixed numeric precision (Hours as decimal?, Amount as decimal)
/// - No CreatedAt / UpdatedAt
/// </summary>
public class PayrollRunDetail
{
    public int DetailId { get; set; }

    /// <summary>No enforced FK — orphaned rows possible.</summary>
    public int PayrollRunId { get; set; }

    /// <summary>No enforced FK — orphaned rows possible.</summary>
    public int EmployeeId { get; set; }

    /// <summary>Free-text earnings/deduction code; no FK to a code table.</summary>
    public string EarningsCode { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public decimal? Hours { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? DeductionAmount { get; set; }

    /// <summary>Free-text override note entered by payroll clerk.</summary>
    public string? OverrideNote { get; set; }
}
