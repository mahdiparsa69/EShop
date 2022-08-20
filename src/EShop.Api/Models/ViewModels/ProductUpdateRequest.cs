namespace EShop.Api.Models.ViewModels
{
    public class ProductUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public int? AvailableCount { get; set; }

        public long? Price { get; set; }

        public byte? DiscountPercent { get; set; }
    }
}
