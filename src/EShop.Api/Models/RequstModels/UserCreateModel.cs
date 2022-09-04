namespace EShop.Api.Models.RequstModels
{
    public class UserCreateModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Msisdn { get; set; }

        public string Address { get; set; }
    }
}
