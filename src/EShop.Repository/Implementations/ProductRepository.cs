using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;

namespace EShop.Repository.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly EShopDbContext _dbContext;

        public ProductRepository(EShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
