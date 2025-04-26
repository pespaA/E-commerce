using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task AddAsync(TEntity entity)
        => await _storeContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
        =>  _storeContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool TrackChanges=false)
        => TrackChanges ? await _storeContext.Set<TEntity>().ToListAsync()
            :await _storeContext.Set<TEntity>().AsNoTracking().ToListAsync();

        #region For Specifications
        public async Task<IEnumerable<TEntity>> GetAllWithSpecificationsAsync(Specifications<TEntity> specifications)

            => await ApplySpecifications(specifications).ToListAsync();

        public async Task<TEntity?> GetByIdWithSpecificationsAsync(Specifications<TEntity> specifications)

            => await ApplySpecifications(specifications).FirstOrDefaultAsync();

        private IQueryable<TEntity> ApplySpecifications(Specifications<TEntity> spec)
        {
            var query = _storeContext.Set<TEntity>().AsQueryable();

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            query = spec.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
        #endregion

        public async Task<TEntity?> GetByIdAsync(TKey Id)
        => await _storeContext.Set<TEntity>().FindAsync(Id);

        public void Update(TEntity entity)
        => _storeContext.Set<TEntity>().Update(entity);
    }
}
