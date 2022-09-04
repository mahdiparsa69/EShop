using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequstModels
{
    public class OrderCreateRequest
    {
        public Guid UserId { get; set; }

        public bool IsPaid { get; set; }

        public long TotalAmount { get; set; }

        public long FinalAmount { get; set; }

        public long DiscountAmount { get; set; }
    }
}
