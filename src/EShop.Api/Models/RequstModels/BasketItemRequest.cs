using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequstModels
{
    public class BasketItemRequest
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        [Range(minimum:1,100)]
        public int Count { get; set; }
    }
}