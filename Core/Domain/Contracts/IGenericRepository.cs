﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity :BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool TrackChanges=false);
        Task<TEntity?> GetByIdAsync(TKey Id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
