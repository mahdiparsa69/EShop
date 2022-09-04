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
            return query;
        }

        public override IQueryable<Transaction> ConfigureInclude(IQueryable<Transaction> query)
        {
            return query;
        }

        public override IQueryable<Transaction> ConfigureListInclude(IQueryable<Transaction> query)
        {
            return query;
        }
    }
}
