using EShop.Domain.Filters;
using EShop.Domain.Models;

namespace EShop.Domain.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<Transaction, TransactionFilter>
    {
    }
}
