using EShop.Domain.Filters;
using EShop.Domain.Models;


namespace EShop.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, UserFilter>
    {
        /// <summary>
        /// Gets a user from database based on its username
        /// </summary>
        /// <returns></returns>
        public Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}
