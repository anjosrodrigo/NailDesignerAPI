namespace NailDesignerAPI.Models {
    public class ServiceAddOn {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PricePerUnit { get; set; }
        public double PriceAll { get; set; }
        public int DurationMinutes { get; set; } = 30;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}