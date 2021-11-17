using Microsoft.EntityFrameworkCore;
using SolarSystem.Data;
using SolarSystem.Data.DTOs;
using SolarSystem.Data.Entities;
using SolarSystem.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace SolarSystem.Repository.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext context;
        private readonly DbSet<T> db;
        public GenericRepository(ApplicationContext context)
        {
            this.context = context;
            db = this.context.Set<T>();
        }

        public async Task<IPagedList<T>> GetAllAsync(PaginationParam request, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = db;

            if(expression is not null)
            {
                query = query.Where(expression);
            }

            if(includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            orderBy?.Invoke(query = orderBy(query));

            // ToDo: look up why as no tracking
            return await query.AsNoTracking().ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = db;

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(T entity)
        {
            await db.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await db.FindAsync(id);

            if (entity is null) throw new Exception("The entity with the provided Id does not exist!");

            db.Remove(entity);
        }

        public void Update(T entity)
        {
            db.Attach(entity);

            context.Entry(entity).State = EntityState.Modified;
        }
    }
}