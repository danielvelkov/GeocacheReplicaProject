using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeocacheV2.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        // methods for finding objects
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        // expression means we can use lambda to filter objects( just like we do with where)
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        // methods for adding
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        // methods for removing
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
