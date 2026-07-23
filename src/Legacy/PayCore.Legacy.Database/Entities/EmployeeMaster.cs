namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Legacy employee master record. Intentional flaws:
/// - SIN stored as plain string (CHAR(10)), not a typed identifier
/// - Bank account details flat on the same row (no separate table)
/// - IsActive is bool? — null means "unknown" (three-state)
/// - No CreatedAt / UpdatedAt audit columns
/// </summary>
public class EmployeeMaster
{
    public int EmployeeId { get; set; }

    public string EmployeeNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    /// <summary>Stored as CHAR(10) — legacy decision, not a typed value object.</summary>
    public string SocialInsuranceNumber { get; set; } = string.Empty;

    public string? DateOfBirth { get; set; }

    public string HireDate { get; set; } = string.Empty;

    public string? TerminationDate { get; set; }

    /// <summary>True = active, False = inactive, null = status unknown (legacy three-state).</summary>
    public bool? IsActive { get; set; }

    public string? Department { get; set; }

    public string? JobTitle { get; set; }

    public decimal? HourlyRate { get; set; }

    public decimal? AnnualSalary { get; set; }

    // Bank details stored flat — no separate BankAccount table
    public string? BankTransitNumber { get; set; }

    public string? BankInstitutionNumber { get; set; }

    public string? BankAccountNumber { get; set; }

    public string Province { get; set; } = string.Empty;
}
