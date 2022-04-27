using System.Linq.Expressions;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{
    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository where T : class
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        Task AddAsync(T entity);

        Task RemoveAsync(int id);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entity);
    }
}