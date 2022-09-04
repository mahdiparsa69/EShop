using EShop.Domain.Enums;

namespace EShop.Api.Models.ViewModels
{
    public class TransactionViewModel
    {
        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public long Amount { get; set; }

        public TransactionStatus Status { get; set; }

        public string ErrorMessage { get; set; }

        public string? PaymentCode { get; set; }
    }
}
