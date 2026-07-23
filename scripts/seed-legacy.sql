-- =============================================================================
-- seed-legacy.sql
-- Minimal seed data for the PayCore_Legacy database.
--
-- Idempotent: safe to run multiple times.
--   DEDUCTION_CODE  — MERGE on Code (inserts only; existing rows are left as-is)
--   COMPANY_SETTINGS — IF NOT EXISTS insert (single-row table, no PK)
--
-- No DELETE, TRUNCATE, or DROP statements.
-- =============================================================================

SET NOCOUNT ON;
GO

-- ── DEDUCTION_CODE ──────────────────────────────────────────────────────────
-- 10 standard deduction codes.  MERGE key is Code (business key).
-- Amount / Percentage are NULL for codes whose values are calculated at
-- run-time by the payroll engine; only PARKING carries a fixed flat amount.

MERGE DEDUCTION_CODE AS target
USING (VALUES
    ('CPP',     'Canada Pension Plan',                'STATUTORY', CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('EI',      'Employment Insurance',               'STATUTORY', CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('FEDTAX',  'Federal Income Tax',                 'STATUTORY', CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('PROVTAX', 'Provincial Income Tax',              'STATUTORY', CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('UNION',   'Union Dues',                         'STATUTORY', CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('HLTH',    'Health Benefits',                    'BENEFIT',   CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('DENTAL',  'Dental Benefits',                    'BENEFIT',   CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('VISION',  'Vision Benefits',                    'BENEFIT',   CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit)),
    ('PARKING', 'Parking Deduction',                  'OTHER',     CAST(0 AS bit), 50.00, NULL,  CAST(1 AS bit)),
    ('RRSP',    'Registered Retirement Savings Plan', 'BENEFIT',   CAST(0 AS bit), NULL,  NULL,  CAST(1 AS bit))
) AS source (Code, Description, DeductionType, IsPercentage, Amount, Percentage, IsActive)
ON target.Code = source.Code
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Code, Description, DeductionType, IsPercentage, Amount, Percentage, IsActive)
    VALUES (source.Code, source.Description, source.DeductionType,
            source.IsPercentage, source.Amount, source.Percentage, source.IsActive);

GO

-- ── COMPANY_SETTINGS ────────────────────────────────────────────────────────
-- Single-row keyless table.  Insert only when the table is empty.

IF NOT EXISTS (SELECT 1 FROM COMPANY_SETTINGS)
BEGIN
    INSERT INTO COMPANY_SETTINGS
        (CompanyName, CompanyNumber, PayrollFrequency, FiscalYearStart, DefaultProvince)
    VALUES
        ('PayCore Sample Company', '001', 'BIWEEKLY', '01-01', 'ON');
END

GO

PRINT 'Seed complete.';
