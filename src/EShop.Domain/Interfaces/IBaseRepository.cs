using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;

namespace EShop.Domain.Interfaces
{
    public interface IBaseRepository<TModelBase> where TModelBase : BaseModel
    {
        //todo add cancellation to repository

        /// <summary>
        /// Create One Object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TModelBase entity, CancellationToken cancellationToken);

        /// <summary>
        /// Create More Than One Object
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TModelBase> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Update One Object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(TModelBase entity, CancellationToken cancellationToken);

        /// <summary>
        /// Update More Than One Object
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void UpdateRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Remove One Object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Remove(TModelBase entity, CancellationToken cancellationToken);

        /// <summary>
        /// Remove One Object By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        Task Remove(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Update More Than One Object
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void RemoveRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Get One Object Based On id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TModelBase?> GetAsync(Guid id, CancellationToken cancellationToken);


        /// <summary>
        /// Get A List Of Objects
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TModelBase>> GetListAsync(CancellationToken cancellationToken);

    }
}
