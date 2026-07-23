namespace PayCore.Legacy.Api.Dtos;

public class PayrollRunDetailDto
{
    public int DetailId { get; set; }
    public int PayrollRunId { get; set; }
    public int EmployeeId { get; set; }
    public string EarningsCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public decimal? Hours { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? DeductionAmount { get; set; }
    public string? OverrideNote { get; set; }
}
