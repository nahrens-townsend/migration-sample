namespace PayCore.Legacy.Api.Dtos;

public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SocialInsuranceNumber { get; set; } = string.Empty;
    public string? DateOfBirth { get; set; }
    public string HireDate { get; set; } = string.Empty;
    public string? TerminationDate { get; set; }
    public bool? IsActive { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? AnnualSalary { get; set; }
    public string? BankTransitNumber { get; set; }
    public string? BankInstitutionNumber { get; set; }
    public string? BankAccountNumber { get; set; }
    public string Province { get; set; } = string.Empty;
}
