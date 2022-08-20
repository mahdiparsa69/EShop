﻿using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;

namespace EShop.Repository.Implementations
{
    public class OrderItemRepository : BaseRepository<OrderItem, OrderItemFilter>, IOrderItemRepository
    {
        private readonly EShopDbContext _dbContext;

        public OrderItemRepository(EShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<OrderItem> ApplyFilter(IQueryable<OrderItem> query, OrderItemFilter filter)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<OrderItem> ConfigureInclude(IQueryable<OrderItem> query)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<OrderItem> ConfigureListInclude(IQueryable<OrderItem> query)
        {
            throw new NotImplementedException();
        }
    }
}
