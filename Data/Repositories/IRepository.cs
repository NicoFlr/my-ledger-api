using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public interface IRepository<TEntity> : IQueryable<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity, byte[]? RowVersion = null);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Detach(TEntity entity);
        void DetachRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
