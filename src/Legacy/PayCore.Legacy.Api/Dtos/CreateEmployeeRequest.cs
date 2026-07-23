namespace PayCore.Legacy.Api.Dtos;

public class CreateEmployeeRequest
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SocialInsuranceNumber { get; set; } = string.Empty;
    public string HireDate { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string? DateOfBirth { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? AnnualSalary { get; set; }
}
