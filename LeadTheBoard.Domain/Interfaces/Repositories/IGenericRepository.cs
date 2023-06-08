using LeadTheBoard.Domain.Entities.Base;
using System.Linq.Expressions;

namespace LeadTheBoard.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T?> FindOneAsync(Expression<Func<T?, bool>> expression, CancellationToken cancellationToken = default);

        IQueryable<T> Find(Expression<Func<T, bool>>? expression = default);

        Task<T> AddAsyncReturnEntity(T entity, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    }
}
