namespace EShop.Api.Models.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public int AvailableCount { get; set; }

        public long Price { get; set; }

        public byte? DiscountPercent { get; set; }


    }
}
