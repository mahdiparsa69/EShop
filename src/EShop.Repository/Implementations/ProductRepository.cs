using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository.Implementations
{
    public class ProductRepository : BaseRepository<Product, ProductFilter>, IProductRepository
    {
        public ProductRepository(EShopDbContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name == filter.Name);
            }
            return query;
        }

        public override IQueryable<Product> ConfigureInclude(IQueryable<Product> query)
        {
            return query;
        }

        public override IQueryable<Product> ConfigureListInclude(IQueryable<Product> query)
        {
            return query;
        }
    }
}
