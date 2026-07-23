namespace PayCore.Legacy.Api.Dtos;

public class CreatePayrollRunRequest
{
    public string PeriodStartDate { get; set; } = string.Empty;
    public string PeriodEndDate { get; set; } = string.Empty;
    public string ProcessedBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
