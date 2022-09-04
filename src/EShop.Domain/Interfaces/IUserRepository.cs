using EShop.Domain.Filters;
using EShop.Domain.Models;


namespace EShop.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, UserFilter>
    {
        public Task<User> GetUser(User user);
    }
}
