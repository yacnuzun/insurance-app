﻿using InsuranceApp.Core;
using InsuranceApp.Core.Entities;
using InsuranceApp.Core.Helpers.ResponseModels;
using InsuranceApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceApp.DataAccess
{
    public class EfRepository<T, TContext> : IRepository<T>
    where T : BaseEntity, IEntity, new()
    where TContext : DbContext
    {
        protected readonly TContext Context;
        public EfRepository(TContext context) => Context = context;

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = Context.Set<T>().Where(x => !x.IsDeleted).FirstOrDefault(predicate);
            return result; 
        }

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                var result = predicate is null ?await Context.Set<T>().Where(x => !x.IsDeleted).ToListAsync()
                    : await Context.Set<T>().Where(x => !x.IsDeleted).Where(predicate).ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<T> AddAsync(T entity)
        {
            return Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            (entity as BaseEntity).IsDeleted = true;
            Update(entity);
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            return Context.Set<T>().AddRange(entities);
        }
    }
}
