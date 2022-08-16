using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly EShopDbContext _dbContext;
        internal DbSet<T> dbSet;
        public Repository(EShopDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"entity  is Null");
            }

            if (entity.Id == null)
            {
                entity.Id = Guid.NewGuid();
            }
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            if (entities.Count == 0)
            {
                throw new ArgumentNullException($"entity  is Null");
            }

            foreach (var entity in entities)
            {
                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid();
                }
            }
            await dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"entity  is Null");
            }
            if (entity.Id == null)
            {
                entity.Id = Guid.NewGuid();
            }
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"entity  is Null");
            }
            dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"entity  is Null");
            }
            dbSet.RemoveRange(entities);
        }

        public async Task<T> GetAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await dbSet.ToListAsync();
            return entities;
        }
    }
}
