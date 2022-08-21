namespace EShop.Api.Models.ViewModels
{
    public class OrderCompactViewModel
    {
        public Guid UserId { get; set; }

        public bool IsPaid { get; set; }

        public long TotalAmount { get; set; }

        public long FinalAmount { get; set; }

        public long DiscountAmount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

    }
}
