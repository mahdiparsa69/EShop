using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;

namespace EShop.Repository.Implementations
{
    public class OrderRepository : BaseRepository<Order, OrderFilter>, IOrderRepository
    {
        private readonly EShopDbContext _dbContext;

        public OrderRepository(EShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<Order> ApplyFilter(IQueryable<Order> query, OrderFilter filter)
        {
            return query;
        }

        public override IQueryable<Order> ConfigureInclude(IQueryable<Order> query)
        {
            return query;
        }

        public override IQueryable<Order> ConfigureListInclude(IQueryable<Order> query)
        {
            return query;
        }
    }
}
