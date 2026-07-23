namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Deduction code lookup table. Intentional flaws:
/// - No effective-dating (no EffectiveFrom / EffectiveTo columns)
/// - No versioning — editing a record silently changes historical meaning
/// - No CreatedAt / UpdatedAt
/// </summary>
public class DeductionCode
{
    public int DeductionCodeId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>Free-text type: "STATUTORY", "BENEFIT", "GARNISHMENT", "OTHER".</summary>
    public string DeductionType { get; set; } = string.Empty;

    public bool IsPercentage { get; set; }

    /// <summary>Flat dollar amount; null when IsPercentage is true.</summary>
    public decimal? Amount { get; set; }

    /// <summary>Percentage (0–100); null when IsPercentage is false.</summary>
    public decimal? Percentage { get; set; }

    public bool IsActive { get; set; }
}
