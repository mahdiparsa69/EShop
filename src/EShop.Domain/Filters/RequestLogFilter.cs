using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Filters
{
    public struct RequestLogFilter : IListFilter
    {
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}
