namespace PayCore.Legacy.Database.Entities;

/// <summary>
/// Audit log for payroll events. Intentional flaws:
/// - EventDetail is an unstructured string blob — no typed schema per event
/// - RecordId stored as string (could be any table's PK, in any format)
/// - UserId is nullable string — anonymous actions are allowed
/// - No indexing guidance; full-table scans expected
/// </summary>
public class AuditLog
{
    public int AuditLogId { get; set; }

    public DateTime Timestamp { get; set; }

    /// <summary>Free-text event type e.g. "PAYROLL_RUN", "EMPLOYEE_UPDATE".</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>Unstructured blob of event detail; null for system-generated entries.</summary>
    public string? EventDetail { get; set; }

    /// <summary>Serialized snapshot or diff — no enforced schema.</summary>
    public string? EventData { get; set; }

    /// <summary>Null when action was performed by an automated job.</summary>
    public string? UserId { get; set; }

    /// <summary>Free-text table name; not an FK.</summary>
    public string? TableAffected { get; set; }

    /// <summary>String representation of the affected record's PK.</summary>
    public string? RecordId { get; set; }
}
