namespace NailDesignerAPI.Models {
    public enum PricingType {
        Full,
        PerUnit,
        Both
    }

    public class ServiceType {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PricingType PricingType { get; set; } = PricingType.Full;
        public double Price { get; set; }
        public double? PricePerUnit { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
