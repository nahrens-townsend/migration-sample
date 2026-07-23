using Microsoft.EntityFrameworkCore;
using PayCore.Legacy.Api.Dtos;
using PayCore.Legacy.Database.Context;
using PayCore.Legacy.Database.Entities;

namespace PayCore.Legacy.Api.Services;

public class LegacyEmployeeService
{
    private readonly LegacyPayrollContext _db;

    public LegacyEmployeeService(LegacyPayrollContext db)
    {
        _db = db;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync(int page, int pageSize)
    {
        var employees = await _db.Employees
            .OrderBy(e => e.EmployeeId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return employees.Select(MapToDto).ToList();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        return employee is null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        var employee = new EmployeeMaster
        {
            EmployeeNumber         = request.EmployeeNumber,
            FirstName              = request.FirstName,
            LastName               = request.LastName,
            SocialInsuranceNumber  = request.SocialInsuranceNumber,
            HireDate               = request.HireDate,
            Province               = request.Province,
            DateOfBirth            = request.DateOfBirth,
            Department             = request.Department,
            JobTitle               = request.JobTitle,
            HourlyRate             = request.HourlyRate,
            AnnualSalary           = request.AnnualSalary,
            IsActive               = true
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null) return null;

        if (request.FirstName          is not null) employee.FirstName          = request.FirstName;
        if (request.LastName           is not null) employee.LastName           = request.LastName;
        if (request.Department         is not null) employee.Department         = request.Department;
        if (request.JobTitle           is not null) employee.JobTitle           = request.JobTitle;
        if (request.HourlyRate         is not null) employee.HourlyRate         = request.HourlyRate;
        if (request.AnnualSalary       is not null) employee.AnnualSalary       = request.AnnualSalary;
        if (request.IsActive           is not null) employee.IsActive           = request.IsActive;
        if (request.TerminationDate    is not null) employee.TerminationDate    = request.TerminationDate;
        if (request.BankTransitNumber  is not null) employee.BankTransitNumber  = request.BankTransitNumber;
        if (request.BankInstitutionNumber is not null) employee.BankInstitutionNumber = request.BankInstitutionNumber;
        if (request.BankAccountNumber  is not null) employee.BankAccountNumber  = request.BankAccountNumber;

        await _db.SaveChangesAsync();

        return MapToDto(employee);
    }

    private static EmployeeDto MapToDto(EmployeeMaster e) => new()
    {
        EmployeeId            = e.EmployeeId,
        EmployeeNumber        = e.EmployeeNumber,
        FirstName             = e.FirstName,
        LastName              = e.LastName,
        SocialInsuranceNumber = e.SocialInsuranceNumber,
        DateOfBirth           = e.DateOfBirth,
        HireDate              = e.HireDate,
        TerminationDate       = e.TerminationDate,
        IsActive              = e.IsActive,
        Department            = e.Department,
        JobTitle              = e.JobTitle,
        HourlyRate            = e.HourlyRate,
        AnnualSalary          = e.AnnualSalary,
        BankTransitNumber     = e.BankTransitNumber,
        BankInstitutionNumber = e.BankInstitutionNumber,
        BankAccountNumber     = e.BankAccountNumber,
        Province              = e.Province
    };
}
