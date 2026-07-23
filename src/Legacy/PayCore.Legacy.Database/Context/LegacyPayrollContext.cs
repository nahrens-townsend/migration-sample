using Microsoft.EntityFrameworkCore;
using PayCore.Legacy.Database.Entities;

namespace PayCore.Legacy.Database.Context;

public class LegacyPayrollContext : DbContext
{
    public LegacyPayrollContext(DbContextOptions<LegacyPayrollContext> options)
        : base(options)
    {
    }

    // DbSet properties — null! is the standard EF Core pattern; EF initialises these via reflection
    public DbSet<EmployeeMaster>    Employees          { get; set; } = null!;
    public DbSet<PayrollRun>        PayrollRuns        { get; set; } = null!;
    public DbSet<PayrollRunDetail>  PayrollRunDetails  { get; set; } = null!;
    public DbSet<DeductionCode>     DeductionCodes     { get; set; } = null!;
    public DbSet<BenefitEnrollment> BenefitEnrollments { get; set; } = null!;
    public DbSet<TaxTable>          TaxTables          { get; set; } = null!;
    public DbSet<CompanySettings>   CompanySettings    { get; set; } = null!;
    public DbSet<AuditLog>          AuditLogs          { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── EMPLOYEE_MASTER ──────────────────────────────────────────────────
        modelBuilder.Entity<EmployeeMaster>(e =>
        {
            e.ToTable("EMPLOYEE_MASTER");
            // EmployeeId is not convention-discoverable (EF expects EmployeeMasterId)
            e.HasKey(x => x.EmployeeId);
            // SIN stored as CHAR(10) — legacy decision
            e.Property(x => x.SocialInsuranceNumber)
             .HasColumnType("char(10)");
        });

        // ── PAYROLL_RUN ──────────────────────────────────────────────────────
        modelBuilder.Entity<PayrollRun>(e =>
        {
            e.ToTable("PAYROLL_RUN");
            // Period dates kept as NVARCHAR(20) strings — not DateTime
            e.Property(x => x.PeriodStartDate)
             .HasColumnType("nvarchar(20)");
            e.Property(x => x.PeriodEndDate)
             .HasColumnType("nvarchar(20)");
        });

        // ── PAYROLL_RUN_DETAIL ───────────────────────────────────────────────
        // Intentionally no FK constraints; orphaned rows are possible
        modelBuilder.Entity<PayrollRunDetail>(e =>
        {
            e.ToTable("PAYROLL_RUN_DETAIL");
            // DetailId is not convention-discoverable (EF expects PayrollRunDetailId)
            e.HasKey(x => x.DetailId);
        });

        // ── DEDUCTION_CODE ───────────────────────────────────────────────────
        modelBuilder.Entity<DeductionCode>(e =>
        {
            e.ToTable("DEDUCTION_CODE");
        });

        // ── BENEFIT_ENROLLMENT ───────────────────────────────────────────────
        // Composite natural key — no surrogate PK
        modelBuilder.Entity<BenefitEnrollment>(e =>
        {
            e.ToTable("BENEFIT_ENROLLMENT");
            e.HasKey(x => new { x.EmployeeId, x.PlanCode });
        });

        // ── TAX_TABLE ────────────────────────────────────────────────────────
        // Composite natural key — duplicated in full per year
        modelBuilder.Entity<TaxTable>(e =>
        {
            e.ToTable("TAX_TABLE");
            e.HasKey(x => new { x.TaxYear, x.Province, x.BracketFloor });
        });

        // ── COMPANY_SETTINGS ─────────────────────────────────────────────────
        // [Keyless] is already declared on the class; no HasKey call needed
        modelBuilder.Entity<CompanySettings>(e =>
        {
            e.ToTable("COMPANY_SETTINGS");
        });

        // ── AUDIT_LOG ────────────────────────────────────────────────────────
        modelBuilder.Entity<AuditLog>(e =>
        {
            e.ToTable("AUDIT_LOG");
        });
    }
}
