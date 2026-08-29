using NailDesignerAPI.Models;

namespace NailDesignerAPI.DTOs {
    public class ServiceTypeDTO {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PricingType PricingType { get; set; } = PricingType.Full;
        public double Price { get; set; }
        public double? PricePerUnit { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CreateServiceTypeDTO {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PricingType PricingType { get; set; } = PricingType.Full;
        public double Price { get; set; }
        public double? PricePerUnit { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateServiceTypeDTO {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PricingType PricingType { get; set; } = PricingType.Full;
        public double Price { get; set; }
        public double? PricePerUnit { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
