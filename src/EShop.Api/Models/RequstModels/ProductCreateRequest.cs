using System.ComponentModel.DataAnnotations;

namespace EShop.Api.Models.RequestModels
{
    public class ProductCreateRequest
    {
        [Required]
        public string Name { get; set; }

        public int AvailableCount { get; set; }

        [Required]
        public long Price { get; set; }

        public byte? DiscountPercent { get; set; }
    }
}
