using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Enums
{
    public enum TransactionStatus
    {
        None = 0,
        Successful = 1,
        UnSuccessful = 2,
    }
}
