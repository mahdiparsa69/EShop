using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EShop.Repository.Implementations
{
    public abstract class BaseRepository<TModelBase> : IBaseRepository<TModelBase>
        where TModelBase : BaseModel
    {
        private readonly EShopDbContext _dbContext;

        private readonly DbSet<TModelBase> _dbSet;

        public BaseRepository(EShopDbContext dbContext)
        {
            _dbContext = dbContext;

            _dbSet = _dbContext.Set<TModelBase>();
        }

        public async Task AddAsync(TModelBase entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} is null");

            if (entity.Id == default)
                entity.Id = Guid.NewGuid();

            await _dbSet.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
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

        public void Update(TModelBase entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)}  is Null");

            EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
                entry = _dbSet.Attach(entity);

            entry.Property(x => x.SeqId).IsModified = false;
            entry.Property(x => x.CreatedAt).IsModified = false;

            entity.ModifiedAt = DateTimeOffset.Now;

            _dbSet.Update(entity);

            _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void UpdateRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
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

        public void Remove(TModelBase entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)}  is Null");

            EntityEntry<TModelBase> entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
                entry = _dbSet.Attach(entity);

            entity.IsDeleted = true;

            entity.DeletedAt = DateTimeOffset.Now;

            _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Remove(Guid id, CancellationToken cancellationToken)
        {
            if (id == default)
                throw new ArgumentNullException($"Id is Null Cannot remove");

            var entity = await GetAsync(id, cancellationToken);

            if (entity == null)
                throw new Exception($"There is no object with given id:{id} in database");

            Remove(entity, cancellationToken);
        }

        public void RemoveRange(IEnumerable<TModelBase> entities, CancellationToken cancellationToken)
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



        public Task<TModelBase> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TModelBase>> GetListAsync(CancellationToken cancellationToken)
        {
            var entities = await _dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return entities;
        }
    }
}
