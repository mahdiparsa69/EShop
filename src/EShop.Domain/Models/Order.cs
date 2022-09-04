namespace EShop.Domain.Models
{
    public class Order : BaseModel
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public bool IsPaid { get; set; }

        public long TotalAmount { get; set; }

        public long DiscountAmount { get; set; }

        public long FinalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
