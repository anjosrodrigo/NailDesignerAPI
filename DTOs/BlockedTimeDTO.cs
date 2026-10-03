namespace NailDesignerAPI.DTOs {
    public class BlockedTimeDTO {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
    }

    public class CreateBlockedTimeDTO {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Reason { get; set; }
    }
}
