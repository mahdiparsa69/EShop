using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Api.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public long TotalAmount { get; set; }

        public long DiscountAmount { get; set; }

        public List<BasketItem> BasketItems { get; set; }




    }
}
