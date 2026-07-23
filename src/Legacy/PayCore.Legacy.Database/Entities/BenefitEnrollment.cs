namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Current benefit enrollment for an employee. Intentional flaws:
/// - Composite natural key (EmployeeId + PlanCode) — no surrogate PK
/// - No history: re-enrolling overwrites the existing row
/// - No EnrollmentDate / TerminationDate audit trail
/// - No FK constraint on EmployeeId
/// </summary>
public class BenefitEnrollment
{
    /// <summary>Part of composite natural key. No enforced FK.</summary>
    public int EmployeeId { get; set; }

    /// <summary>Part of composite natural key.</summary>
    public string PlanCode { get; set; } = string.Empty;

    public string PlanDescription { get; set; } = string.Empty;

    public decimal? CoverageAmount { get; set; }

    public decimal? EmployeeContribution { get; set; }

    public decimal? EmployerContribution { get; set; }

    public string? BenefitProvider { get; set; }
}
