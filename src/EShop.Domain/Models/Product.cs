using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }

        public int AvailableCount { get; set; }

        public long Price { get; set; }

        public byte? DiscountPercent { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
