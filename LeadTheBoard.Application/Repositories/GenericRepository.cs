using LeadTheBoard.Application.Contexts;
using LeadTheBoard.Domain.Entities.Base;
using LeadTheBoard.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LeadTheBoard.Application.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _appDbContext;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task<T> AddAsyncReturnEntity(T entity, CancellationToken cancellationToken = default)
        {
            var data = await _appDbContext.Set<T>().AddAsync(entity, cancellationToken);
            return data.Entity;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                _appDbContext.Set<T>().Remove(entity);
            }, cancellationToken);
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    _appDbContext.Set<T>().Remove(entity);
                }
            }, cancellationToken);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>>? expression = default)
        {
            return expression == null ? _appDbContext.Set<T>() : _appDbContext.Set<T>().Where(expression);
        }

        public virtual Task<T?> FindOneAsync(Expression<Func<T?, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
        }

        public virtual Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _appDbContext.Set<T>().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                _appDbContext.Set<T>().Update(entity);
            }, cancellationToken);
        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                foreach (var entity in entities)
                {
                    _appDbContext.Set<T>().Update(entity);
                }
            }, cancellationToken);
        }
    }
}
