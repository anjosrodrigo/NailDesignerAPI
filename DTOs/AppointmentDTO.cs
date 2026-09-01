using NailDesignerAPI.Models;

namespace NailDesignerAPI.DTOs {
    public class AppointmentDTO {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ServiceTypeName { get; set; } = string.Empty;
        public double ServicePrice { get; set; }
        public double Discount { get; set; } = 0;
        public double TotalPrice { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public CancellationReason? CancellationReason { get; set; }
        public string? CancellationNotes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<AppointmentAddOnItemDTO> AddOns { get; set; } = new List<AppointmentAddOnItemDTO>();

    }

    public class CreateAppointmentDTO {
        public int ClientId { get; set; }
        public int ServiceTypeId { get; set; }
        public double Discount { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public List<CreateAppointmentAddOnDTO> AddOns { get; set; } = new List<CreateAppointmentAddOnDTO>();
    }

    public class UpdateAppointmentDTO {
        public int ServiceTypeId { get; set; }
        public double Discount { get; set; } = 0;
        public DateTime StartTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public CancellationReason? CancellationReason { get; set; }
        public string? CancellationNotes { get; set; }
        public List<CreateAppointmentAddOnDTO> AddOns { get; set; } = new List<CreateAppointmentAddOnDTO>();
    }

    public class CreateAppointmentAddOnDTO {
        public int ServiceAddOnId { get; set; }
        public int Quantity { get; set; }
    }

    public class AppointmentAddOnItemDTO {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
