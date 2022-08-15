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
        [Display(Name = "موفقیت آمیز")]
        Successful = 0,
        [Display(Name = "ناموفق")]
        UnSuccessful = 1,

    }
}
