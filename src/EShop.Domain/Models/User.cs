using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models
{
    public class User : BaseModel
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Msisdn { get; set; }
        public string Address { get; set; }

        #region NavigationProperty
        public List<Order> Orders { get; set; }
        public List<Transaction> Transactions { get; set; }
        #endregion
    }
}
