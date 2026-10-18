namespace trpo_6.Data
{
    public class MedicineDto
    {
        public string? Name { get; set; } = null!;

        public string? Manufacturer { get; set; } = null!;

        public decimal? Price { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
