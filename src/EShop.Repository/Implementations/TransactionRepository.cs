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
    public class TransactionRepository : BaseRepository<Transaction, TransactionFilter>, ITransactionRepository
    {
        private readonly EShopDbContext _dbContext;

        public TransactionRepository(EShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IQueryable<Transaction> ApplyFilter(IQueryable<Transaction> query, TransactionFilter filter)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Transaction> ConfigureInclude(IQueryable<Transaction> query)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Transaction> ConfigureListInclude(IQueryable<Transaction> query)
        {
            throw new NotImplementedException();
        }
    }
}
