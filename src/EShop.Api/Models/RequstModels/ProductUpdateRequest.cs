namespace EShop.Api.Models.RequestModels
{
    public class ProductUpdateRequest
    {
        public string Name { get; set; }

        public int AvailableCount { get; set; }

        public long Price { get; set; }

        public byte DiscountPercent { get; set; }
    }
}
