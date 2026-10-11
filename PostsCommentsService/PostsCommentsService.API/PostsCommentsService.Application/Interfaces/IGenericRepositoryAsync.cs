using System.Linq.Expressions;

namespace PostsCommentsService.Data.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task DeleteRangeAsync(ICollection<T> entities, CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        IQueryable<T> GetTableNoTracking(CancellationToken cancellationToken);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        IQueryable<T> GetTableAsTracking(CancellationToken cancellationToken);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);

    }
}
