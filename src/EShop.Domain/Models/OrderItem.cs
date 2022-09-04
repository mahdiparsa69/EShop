using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Domain.Models
{
    public class OrderItem : BaseModel
    {
        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int Count { get; set; }

        public long ProductPrice { get; set; }

        public byte? ProductDiscountPercent { get; set; }

        public long TotalPrice { get; set; }

        [NotMapped]
        public bool HasDiscount => ProductDiscountPercent != null || ProductDiscountPercent != 0;
    }
}
