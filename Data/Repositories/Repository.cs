using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repositories
{
   public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        
        private readonly MyLedgerDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(MyLedgerDbContext dbContext)
        {
            _dbContext = dbContext;         
            _dbSet = _dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        
        public void Update(TEntity entity, byte[]? RowVersion = null)
        {
            if (RowVersion != null)
            {
                _dbContext.Entry<TEntity>(entity).OriginalValues["RowVersion"] = RowVersion;
            }

            if (_dbContext.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
            }

            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry<TEntity>(entity).State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_dbSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_dbSet).GetEnumerator();
        }

        public void Detach(TEntity entity)
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Detached;
        }
        public void DetachRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _dbContext.Entry<TEntity>(entity).State = EntityState.Detached;
            }
            
        }


        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (_dbContext.Entry<TEntity>(entity).State == EntityState.Detached)
                {
                    _dbContext.Attach(entity);
                }
            }

            _dbSet.RemoveRange(entities);
        }

        public Type ElementType
        {
            get
            {
                return _dbSet.AsQueryable().ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return _dbSet.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _dbSet.AsQueryable().Provider;
            }
        }
    }
}
