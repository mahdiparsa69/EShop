using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EShop.Domain.Models
{
    public class Transaction : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public long TotalPrice { get; set; }
        public bool Status { get; set; }
        public string? PaymentCode { get; set; }
    }
}
