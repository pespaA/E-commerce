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

        public async Task<TEntity?> GetByIdAsync(TKey Id)
        => await _storeContext.Set<TEntity>().FindAsync(Id);

        public void Update(TEntity entity)
        => _storeContext.Set<TEntity>().Update(entity);
    }
}
