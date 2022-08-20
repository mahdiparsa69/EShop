using EShop.Domain.Common;
using EShop.Domain.Filters;
using EShop.Domain.Models;

namespace EShop.Domain.Interfaces
{
    public interface IBaseRepository<TModelBase, TFilter> where TModelBase : BaseModel
        where TFilter : struct, IListFilter
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
        Task Update(TModelBase entity, CancellationToken cancellationToken);

        /// <summary>
        /// Update More Than One Object
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Remove One Object 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Remove(TModelBase entity, CancellationToken cancellationToken);

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
        Task RemoveRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Get One Object Based On id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TModelBase?> GetAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Get One Object Based On id Without Include
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TModelBase?> GetWithoutIncludeAsync(Guid id, CancellationToken cancellationToken);



        /// <summary>
        /// Get A List Of Objects
        /// </summary>
        /// <returns></returns>
        Task<PaginatedResult<TModelBase>> GetListAsync(TFilter filter, CancellationToken cancellationToken);


    }
}
