using SolarSystem.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SolarSystem.Repository.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
        Task<IPagedList<TEntity>> GetAllAsync(
            PaginationParam request,
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<string> includes = null
            );

        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> expression,
            List<string> includes = null
            );

        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(int id);
    }
}
