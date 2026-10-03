namespace NailDesignerAPI.DTOs {
    public class DayWorkloadDTO {
        public DateOnly Date { get; set; }
        public int OccupiedMinutes { get; set; }
        public int AvailableMinutes { get; set; }
        public bool IsFull { get; set; }
    }
}