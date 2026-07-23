namespace PayCore.Legacy.Api.Dtos;

public class PayrollRunDto
{
    public int PayrollRunId { get; set; }
    public DateTime RunDate { get; set; }
    public string PeriodStartDate { get; set; } = string.Empty;
    public string PeriodEndDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalGross { get; set; }
    public decimal TotalNet { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalTax { get; set; }
    public string ProcessedBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
