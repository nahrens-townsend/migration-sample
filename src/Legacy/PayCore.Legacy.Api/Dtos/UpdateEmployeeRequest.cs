namespace PayCore.Legacy.Api.Dtos;

public class UpdateEmployeeRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? AnnualSalary { get; set; }
    public bool? IsActive { get; set; }
    public string? TerminationDate { get; set; }
    public string? BankTransitNumber { get; set; }
    public string? BankInstitutionNumber { get; set; }
    public string? BankAccountNumber { get; set; }
}
