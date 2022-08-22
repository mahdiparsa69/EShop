using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Api.Models
{
    public class BasketItem
    {
        public Guid BasketId { get; set; }

        public Guid ProductId { get; set; }

        public long Price { get; set; }

        public int Count { get; set; }

        public bool HasDiscount { get; set; }

        public int? ItemDiscount { get; set; }

        public long TotalPrice { get; set; }




    }
}
