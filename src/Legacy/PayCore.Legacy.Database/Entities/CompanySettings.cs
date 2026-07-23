using Microsoft.EntityFrameworkCore;

namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Single-row configuration table. Intentional flaw:
/// - [Keyless] because the original schema has no PK — the application assumes exactly one row
/// </summary>
[Keyless]
public class CompanySettings
{
    public string CompanyName { get; set; } = string.Empty;

    public string CompanyNumber { get; set; } = string.Empty;

    /// <summary>Free-text: "WEEKLY", "BIWEEKLY", "SEMI_MONTHLY", "MONTHLY".</summary>
    public string PayrollFrequency { get; set; } = string.Empty;

    /// <summary>Legacy string date e.g. "01-01".</summary>
    public string FiscalYearStart { get; set; } = string.Empty;

    public string DefaultProvince { get; set; } = string.Empty;

    public string? RemittanceAccount { get; set; }

    public string? ContactEmail { get; set; }

    public string? PhoneNumber { get; set; }
}
