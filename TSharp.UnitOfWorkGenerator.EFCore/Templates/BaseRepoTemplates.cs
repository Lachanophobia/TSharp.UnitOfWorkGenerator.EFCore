using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{
    internal static partial class BuildTemplate
    {
        public static string BuildBaseRepoTemplate(this Template templateBaseRepo)
        {
            var template = @"// Auto-generated code
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
{0}

namespace {1};

public partial class Repository<T> : IRepository<T> where T : class
{{
    private readonly {2} _db;
    internal DbSet<T> dbSet;

    public Repository({2} db)
    {{
        _db = db;
        dbSet = _db.Set<T>();
    }}

    #region asynchronous methods

    /// <inheritdoc/>
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {{
        await dbSet.AddAsync(entity, cancellationToken);
    }}

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {{
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }}

    /// <inheritdoc/>
    public virtual async Task<T> GetAsync({3} id, CancellationToken cancellationToken = default)
    {{
        return await dbSet.FindAsync(id, cancellationToken);
    }}

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
    {{
        IQueryable<T> query = dbSet;

        if (filter != null)
        {{
            query = query.Where(filter);
        }}

        if (includeProperties.Any())
        {{
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {{
                query = query.Include(expression);
            }}
        }}

        if (orderBy != null)
        {{
            return await query.OrderBy(orderBy).ToListAsync(cancellationToken);
        }}

        return await query.ToListAsync(cancellationToken);
    }}

    /// <inheritdoc/>
    public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
    {{
        IQueryable<T> query = dbSet;

        if (filter != null)
        {{
            query = query.Where(filter);
        }}

        if (includeProperties.Any())
        {{
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {{
                query = query.Include(expression);
            }}
        }}

        return await query.FirstOrDefaultAsync(cancellationToken);
    }}

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(T entity)
    {{
        dbSet.Update(entity);
    }}

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entity)
    {{
        dbSet.UpdateRange(entity);
    }}

    /// <inheritdoc/>
    public virtual async Task RemoveAsync({3} id, CancellationToken cancellationToken = default)
    {{
        T entity = await dbSet.FindAsync(id, cancellationToken);
        dbSet.Remove(entity);
    }}

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(T entity)
    {{
        dbSet.Remove(entity);
    }}

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAsync(IEnumerable<T> entity)
    {{
        dbSet.RemoveRange(entity);
    }}

    #endregion

    #region synchronous methods

    /// <inheritdoc/>
    public virtual T Get({3} id)
    {{
        return dbSet.Find(id);
    }}

    /// <inheritdoc/>
    public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
    {{
        IQueryable<T> query = dbSet;

        if (filter != null)
        {{
            query = query.Where(filter);
        }}

        if (includeProperties.Any())
        {{
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {{
                query = query.Include(expression);
            }}
        }}

        if (orderBy != null)
        {{
            return query.OrderBy(orderBy).ToList();
        }}

        return query.ToList();
    }}

    /// <inheritdoc/>
    public virtual T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
    {{
        IQueryable<T> query = dbSet;

        if (filter != null)
        {{
            query = query.Where(filter);
        }}

        if (includeProperties.Any())
        {{
            var expressions = includeProperties.Select(ex => ex);

            foreach (var expression in expressions)
            {{
                query = query.Include(expression);
            }}
        }}

        return query.FirstOrDefault();
    }}

    /// <inheritdoc/>
    public virtual void Add(T entity)
    {{
        dbSet.Add(entity);
    }}

    /// <inheritdoc/>
    public virtual void AddRange(IEnumerable<T> entities)
    {{
        dbSet.AddRange(entities);
    }}

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {{
        dbSet.Update(entity);
    }}

    /// <inheritdoc/>
    public virtual void UpdateRange(IEnumerable<T> entity)
    {{
        dbSet.UpdateRange(entity);
    }}

    /// <inheritdoc/>
    public virtual void Remove({3} id)
    {{
        T entity = dbSet.Find(id);
        dbSet.Remove(entity);
    }}

    /// <inheritdoc/>
    public virtual void Remove(T entity)
    {{
        dbSet.Remove(entity);
    }}

    /// <inheritdoc/>
    public virtual void RemoveRange(IEnumerable<T> entity)
    {{
        dbSet.RemoveRange(entity);
    }}

    #endregion
}}

";

            var baseRepoTemplate = string.Format(template, templateBaseRepo.UsingStatements, templateBaseRepo.Namespace, templateBaseRepo.DBContextName, templateBaseRepo.IdentityColumn);

            return baseRepoTemplate;
        }


        public static string BuildIBaseRepoTemplate(this Template templateBaseIRepo)
        {
            var template = @"// Auto-generated code
using System.Linq.Expressions;

namespace {0}
{{
    public interface IRepository
    {{
    }}

    public partial interface IRepository<T> : IRepository where T : class
    {{
        #region asynchronous methods

        /// <summary>
        /// Gets the entity by id
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""id""></param>
        /// <returns><typeparamref name=""T""/></returns>
        Task<T> GetAsync({1} id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name=""T""/>
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""orderBy"">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns>IEnumerable of <typeparamref name=""T""/></returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns><typeparamref name=""T""/></returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds a new entity to database
        /// </summary>
        /// <typeparam name=""T"">The type of the entity to add</typeparam>
        /// <param name=""entity""></param>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins tracking the given entities, and any other reachable entities that are
        /// not already being tracked, in the EntityState.Added"" state such that they will
        /// be inserted into the database when DbContext.SaveChanges() is called.
        /// </summary>
        /// <param name=""entities""></param>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <typeparam name=""T"">The type of the entity to update</typeparam>
        /// <param name=""entity""></param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <typeparam name=""T"">The type of the IEnumerable entities to update</typeparam>
        /// <param name=""entity""></param>
        Task UpdateRangeAsync(IEnumerable<T> entity);

        /// <summary>
        /// Deletes the entity by its id
        /// </summary>
        /// <param name=""id""></param>
        Task RemoveAsync({1} id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <typeparam name=""T"">The type to delete</typeparam>
        /// <param name=""entity""></param>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Deletes a range of entities
        /// </summary>
        /// <typeparam name=""T"">The type of the IEnumerable entities to delete</typeparam>
        /// <param name=""entity""></param>
        Task RemoveRangeAsync(IEnumerable<T> entity);

        #endregion

        #region synchronous methods

        /// <summary>
        /// Gets the entity by id
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""id""></param>
        /// <returns><typeparamref name=""T""/></returns>
        T Get({1} id);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name=""T""/>
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""orderBy"">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns><typeparamref name=""T""/></returns>
        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds a new entity to database
        /// </summary>
        /// <typeparam name=""T"">The type of the entity to add</typeparam>
        /// <param name=""entity""></param>
        void Add(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <typeparam name=""T"">The type of the entity to update</typeparam>
        /// <param name=""entity""></param>
        void Update(T entity);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <typeparam name=""T"">The type of the IEnumerable entities to update</typeparam>
        /// <param name=""entity""></param>
        void UpdateRange(IEnumerable<T> entity);


        /// <summary>
        /// Deletes the entity by its id
        /// </summary>
        /// <param name=""id""></param>
        void Remove({1} id);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <typeparam name=""T"">The type to delete</typeparam>
        /// <param name=""entity""></param>
        void Remove(T entity);

        /// <summary>
        /// Deletes a range of entities
        /// </summary>
        /// <typeparam name=""T"">The type of the IEnumerable entities to delete</typeparam>
        /// <param name=""entity""></param>
        void RemoveRange(IEnumerable<T> entity);

        /// <summary>
        /// Begins tracking the given entities, and any other reachable entities that are
        /// not already being tracked, in the EntityState.Added state such that they will
        /// be inserted into the database when DbContext.SaveChanges() is called.
        /// </summary>
        /// <param name=""entities""></param>
        void AddRange(IEnumerable<T> entities);

        #endregion
    }}
}}
";

            var baseIRepoTemplate = string.Format(template, templateBaseIRepo.Namespace, templateBaseIRepo.IdentityColumn);

            return baseIRepoTemplate;
        }
    }
}
