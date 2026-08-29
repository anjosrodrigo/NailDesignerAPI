namespace NailDesignerAPI.Models {
    public enum AuditAction {
        Create,
        Update,
        Delete
    }

    public class AuditLog {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public AuditAction Action { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public string ChangedBy { get; set; } = "System";
    }
}
