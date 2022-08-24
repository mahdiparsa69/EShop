using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Api.Models.ViewModels;

namespace EShop.Api.Models
{
    public class BasketItem
    {
        public ProductViewModel Product { get; set; }

        public int Count { get; set; }

        public long Amount { get; set; }

        public long DiscountAmount { get; set; }

        public long FinalAmount { get; set; }
    }
}
