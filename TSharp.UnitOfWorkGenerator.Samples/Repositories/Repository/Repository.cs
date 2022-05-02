using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.Samples.Entities;
using TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.Repository;

public partial class Repository<T> : IRepository<T> where T : class
{
    private readonly TSharpContext _db;
    internal DbSet<T> dbSet;

    public Repository(TSharpContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    #region asynchronous methods

    /// <inheritdoc/>
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(id, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties.Any())
        {
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }
        }

        if (orderBy != null)
        {
            return await query.OrderBy(orderBy).ToListAsync(cancellationToken);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties.Any())
        {
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
    }

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entity)
    {
        dbSet.UpdateRange(entity);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        T entity = await dbSet.FindAsync(id, cancellationToken);
        dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAsync(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

    #endregion

    #region synchronous methods

    /// <inheritdoc/>
    public virtual T Get(int id)
    {
        return dbSet.Find(id);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties.Any())
        {
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }
        }

        if (orderBy != null)
        {
            return query.OrderBy(orderBy).ToList();
        }

        return query.ToList();
    }

    /// <inheritdoc/>
    public virtual T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties.Any())
        {
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {
                query = query.Include(expression);
            }
        }

        return query.FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual void Add(T entity)
    {
        dbSet.Add(entity);
    }

    /// <inheritdoc/>
    public virtual void AddRange(IEnumerable<T> entities)
    {
        dbSet.AddRange(entities);
    }

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {
        dbSet.Update(entity);
    }

    /// <inheritdoc/>
    public virtual void UpdateRange(IEnumerable<T> entity)
    {
        dbSet.UpdateRange(entity);
    }

    /// <inheritdoc/>
    public virtual void Remove(int id)
    {
        T entity = dbSet.Find(id);
        dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

    #endregion
}
