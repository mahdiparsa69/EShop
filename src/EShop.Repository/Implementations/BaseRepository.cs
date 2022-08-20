using EShop.Domain.Common;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EShop.Repository.Implementations
{
    public abstract class BaseRepository<TModelBase, TFilter> : IBaseRepository<TModelBase, TFilter>
        where TModelBase : BaseModel
        where TFilter : struct, IListFilter
    {
        private readonly EShopDbContext _dbContext;

        private readonly DbSet<TModelBase> _dbSet;

        public virtual IQueryable<TModelBase> DefaultSortFunc(IQueryable<TModelBase> query)
        {
            return query.OrderByDescending(x => x.SeqId);
        }

        public abstract IQueryable<TModelBase> ApplyFilter(IQueryable<TModelBase> query, TFilter filter);

        public abstract IQueryable<TModelBase> ConfigureInclude(IQueryable<TModelBase> query);

        public abstract IQueryable<TModelBase> ConfigureListInclude(IQueryable<TModelBase> query);

        public BaseRepository(EShopDbContext dbContext)
        {
            _dbContext = dbContext;

            _dbSet = _dbContext.Set<TModelBase>();
        }

        public virtual async Task AddAsync(TModelBase entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} is null");

            if (entity.Id == default)
                entity.Id = Guid.NewGuid();

            await _dbSet.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
        {

            if (entities == null)
                throw new ArgumentNullException($"{nameof(entities)} are null");

            entities = entities.ToList();

            if (!entities.Any())
                throw new ArgumentNullException($"No Item in {nameof(entities)} for add to db");

            foreach (var entity in entities.Where(x => x.Id == default))
                entity.Id = Guid.NewGuid();

            await _dbSet.AddRangeAsync(entities, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Update(TModelBase entity, CancellationToken cancellationToken)
        {

            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)}  is Null");

            EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
                entry = _dbSet.Attach(entity);

            entry.Property(x => x.SeqId).IsModified = false;

            entry.Property(x => x.CreatedAt).IsModified = false;

            entity.ModifiedAt = DateTime.UtcNow;

            _dbSet.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
        {
            if (entities == null)
                throw new ArgumentNullException($"{nameof(entities)} are null");

            entities = entities.ToList();

            if (!entities.Any())
                throw new ArgumentNullException($"No Item in {nameof(entities)} for update in db");

            foreach (var entity in entities)
            {
                EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

                if (entry.State == EntityState.Detached)
                    entry = _dbSet.Attach(entity);

                entry.Property(x => x.SeqId).IsModified = false;
                entry.Property(x => x.CreatedAt).IsModified = false;

                entity.ModifiedAt = DateTimeOffset.Now;
            }

            _dbSet.UpdateRange(entities);
            _dbContext.SaveChangesAsync(cancellationToken);

        }

        public virtual async Task Remove(TModelBase entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)}  is Null");

            EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
                entry = _dbSet.Attach(entity);

            entity.IsDeleted = true;

            //entity.DeletedAt = DateTimeOffset.Now;
            entity.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Remove(Guid id, CancellationToken cancellationToken)
        {
            if (id == default)
                throw new ArgumentNullException($"Id is Null. Cannot remove");

            var entity = await GetWithoutIncludeAsync(id, cancellationToken);

            if (entity == null)
                throw new Exception($"There is no object with given id:{id} in database");

            Remove(entity, cancellationToken);
        }

        public virtual async Task RemoveRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($" {nameof(entities)} are null");
            }

            entities = entities.ToList();

            if (!entities.Any())
            {
                throw new ArgumentNullException($"no item in {nameof(entities)} for delete from db");
            }

            foreach (var entity in entities)
            {
                EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

                if (entry.State == EntityState.Detached)
                    entry = _dbSet.Attach(entity);

                entity.IsDeleted = true;

                entity.DeletedAt = DateTimeOffset.Now;
            }

            _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task<TModelBase> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbSet
                .Apply(ConfigureInclude)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<TModelBase?> GetWithoutIncludeAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return result;
        }

        public virtual async Task<PaginatedResult<TModelBase>> GetListAsync(TFilter filter, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            query = ApplyFilter(query, filter);

            query = query
                .Apply(ConfigureListInclude)
                .AsNoTracking()
                .Apply(DefaultSortFunc)
                .Skip(filter.Offset)
                .Take(filter.Count);

            var result = new PaginatedResult<TModelBase>
            {
                Items = await query.ToListAsync(cancellationToken),
                TotalCount = await query.CountAsync(cancellationToken)
            };
            return result;
        }



    }
}
