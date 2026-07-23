namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Tax bracket lookup. Intentional flaws:
/// - Composite natural key (TaxYear + Province + BracketFloor) — no surrogate PK
/// - All brackets duplicated in full for each year (no delta/override model)
/// - No CreatedAt / UpdatedAt
/// </summary>
public class TaxTable
{
    /// <summary>Part of composite natural key.</summary>
    public int TaxYear { get; set; }

    /// <summary>Part of composite natural key.</summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>Part of composite natural key. Lower bound of this bracket (inclusive).</summary>
    public decimal BracketFloor { get; set; }

    /// <summary>Upper bound of this bracket; null for the top bracket.</summary>
    public decimal? BracketCeiling { get; set; }

    /// <summary>Marginal rate as a percentage (e.g. 20.50 for 20.5%).</summary>
    public decimal TaxRate { get; set; }

    /// <summary>Fixed surtax or credit applied to this bracket.</summary>
    public decimal? FlatAmount { get; set; }

    public string TaxType { get; set; } = string.Empty;
}
