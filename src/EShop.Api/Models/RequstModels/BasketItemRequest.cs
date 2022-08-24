namespace EShop.Api.Models.RequstModels
{
    public class BasketItemRequest
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; }
    }
}
