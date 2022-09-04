using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository.Implementations
{
    public class UserRepository : BaseRepository<User, UserFilter>, IUserRepository
    {
        private readonly EShopDbContext _dbContext;

        public UserRepository(EShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<User> ApplyFilter(IQueryable<User> query, UserFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Username))
                query = query.Where(x => x.Username == filter.Username);

            return query;
        }

        public override IQueryable<User> ConfigureInclude(IQueryable<User> query)
        {
            return query;
        }

        public override IQueryable<User> ConfigureListInclude(IQueryable<User> query)
        {
            return query;
        }

        public Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        }
    }
}
