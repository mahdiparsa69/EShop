using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;

namespace EShop.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseModel
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(List<T> entities);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(List<T> entities);

        Task<T> GetAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

    }
}
