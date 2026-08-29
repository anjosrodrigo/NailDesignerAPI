namespace NailDesignerAPI.DTOs {
    public class ServiceAddOnDTO {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PricePerUnit { get; set; }
        public double PriceAll { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateServiceAddOnDTO {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PricePerUnit { get; set; }
        public double PriceAll { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateServiceAddOnDTO {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PricePerUnit { get; set; }
        public double PriceAll { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
    }
}
