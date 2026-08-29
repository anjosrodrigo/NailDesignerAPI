namespace NailDesignerAPI.Models {
    public enum AppointmentStatus {
        Scheduled,  // agendado
        Confirmed,  // confirmou WhatsApp
        Cancelled,  // cancelado (ver CancellationReason)
        Completed   // serviço realizado
    }

    public enum CancellationReason {
        NoResponse,         // não respondeu
        ClientCancelled,    // cliente cancelou
        NoShow,             // não apareceu
        Other               // outro motivo
    }

    public class Appointment {
        public int Id { get; set; }

        // Relationships
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; } = null!;

        // Scheduling
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Status
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public CancellationReason? CancellationReason { get; set; }
        public string? CancellationNotes { get; set; }

        // Pricing
        public double TotalPrice { get; set; }
        public double Discount { get; set; } = 0;

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<AppointmentAddOn> AddOns { get; set; } = new List<AppointmentAddOn>();
    }

    public class AppointmentAddOn {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public int ServiceAddOnId { get; set; }
        public ServiceAddOn ServiceAddOn { get; set; } = null!;

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
