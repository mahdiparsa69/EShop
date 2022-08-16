using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;

namespace EShop.Domain.Interfaces
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
    }
}
