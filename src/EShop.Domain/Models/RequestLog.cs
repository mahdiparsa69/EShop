using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models
{
    public class RequestLog : BaseModel
    {
        public string AccessToken { get; set; }
        public string Host { get; set; }
    }
}
