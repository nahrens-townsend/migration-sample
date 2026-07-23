namespace PayCore.Legacy.Api.Dtos;

public class YtdSummaryDto
{
    public int EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public decimal TotalGross { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalNet { get; set; }
}
