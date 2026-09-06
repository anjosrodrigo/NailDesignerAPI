namespace NailDesignerAPI.DTOs {
    public class RevenueDTO {

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double TotalRevenue { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalCancelled { get; set; }
        public int TotalScheduled { get; set; }
        public int TotalConfirmed { get; set; }
        public List<AppointmentDTO> Appointments { get; set; } = new List<AppointmentDTO>();
    }
}
