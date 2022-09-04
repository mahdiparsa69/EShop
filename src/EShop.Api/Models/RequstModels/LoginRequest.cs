using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequstModels
{
    public class LoginRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "")]
        public string UserName { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "")]
        public string Password { get; set; }
    }
}
