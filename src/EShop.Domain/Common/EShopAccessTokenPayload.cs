namespace EShop.Domain.Common
{
    public class EShopAccessTokenPayload
    {
        public Guid userId { get; set; }
        public DateTime ExpireTokenTime { get; set; }
    }
}
