using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequstModels
{
    public class BasketItemRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
