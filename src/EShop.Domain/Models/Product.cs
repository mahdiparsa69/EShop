namespace EShop.Domain.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }

        public int AvailableCount { get; set; }

        public long Price { get; set; }

        public byte? DiscountPercent { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
