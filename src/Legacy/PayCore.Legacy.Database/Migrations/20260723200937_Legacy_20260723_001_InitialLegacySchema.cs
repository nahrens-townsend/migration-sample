using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayCore.Legacy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Legacy_20260723_001_InitialLegacySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDIT_LOG",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableAffected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_LOG", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "BENEFIT_ENROLLMENT",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PlanCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployerContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BenefitProvider = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BENEFIT_ENROLLMENT", x => new { x.EmployeeId, x.PlanCode });
                });

            migrationBuilder.CreateTable(
                name: "COMPANY_SETTINGS",
                columns: table => new
                {
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayrollFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiscalYearStart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultProvince = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemittanceAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DEDUCTION_CODE",
                columns: table => new
                {
                    DeductionCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeductionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEDUCTION_CODE", x => x.DeductionCodeId);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_MASTER",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialInsuranceNumber = table.Column<string>(type: "char(10)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TerminationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AnnualSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankTransitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankInstitutionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_MASTER", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "PAYROLL_RUN",
                columns: table => new
                {
                    PayrollRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEndDate = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PeriodStartDate = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGross = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProcessedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYROLL_RUN", x => x.PayrollRunId);
                });

            migrationBuilder.CreateTable(
                name: "PAYROLL_RUN_DETAIL",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayrollRunId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EarningsCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OverrideNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYROLL_RUN_DETAIL", x => x.DetailId);
                });

            migrationBuilder.CreateTable(
                name: "TAX_TABLE",
                columns: table => new
                {
                    TaxYear = table.Column<int>(type: "int", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BracketFloor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BracketCeiling = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAX_TABLE", x => new { x.TaxYear, x.Province, x.BracketFloor });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "BENEFIT_ENROLLMENT");

            migrationBuilder.DropTable(
                name: "COMPANY_SETTINGS");

            migrationBuilder.DropTable(
                name: "DEDUCTION_CODE");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_MASTER");

            migrationBuilder.DropTable(
                name: "PAYROLL_RUN");

            migrationBuilder.DropTable(
                name: "PAYROLL_RUN_DETAIL");

            migrationBuilder.DropTable(
                name: "TAX_TABLE");
        }
    }
}
