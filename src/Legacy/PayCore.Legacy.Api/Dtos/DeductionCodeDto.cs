namespace PayCore.Legacy.Api.Dtos;

public class DeductionCodeDto
{
    public int DeductionCodeId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeductionType { get; set; } = string.Empty;
    public bool IsPercentage { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Percentage { get; set; }
    public bool IsActive { get; set; }
}
