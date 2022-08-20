using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;

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
            throw new NotImplementedException();
        }

        public override IQueryable<User> ConfigureInclude(IQueryable<User> query)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<User> ConfigureListInclude(IQueryable<User> query)
        {
            throw new NotImplementedException();
        }
    }
}
