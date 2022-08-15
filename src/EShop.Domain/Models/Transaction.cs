using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Enums;


namespace EShop.Domain.Models
{
    public class Transaction : BaseModel
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public long Amount { get; set; }

        public TransactionStatus Status { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }

        public string? PaymentCode { get; set; }
    }
}
