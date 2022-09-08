using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequstModels
{
    public class UserCreateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Msisdn { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
