using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.DataAccess.Entities;
using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.EFCore.Tests.TestOverrides
{
    public static class SourceInfo
    {
        public static string GetSourceTrees()
        {
            return @"
public class Program
{
    static void Main(string[] args)
    {
        }
    }


namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    using TSharp.UnitOfWorkGenerator.EFCore.Utils;

    [UoWGenerateRepository]
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}




namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    using Microsoft.EntityFrameworkCore;
    using TSharp.UnitOfWorkGenerator.EFCore.Utils;

    [UoWDefineDbContext]
    public class TSharpContext : DbContext
    {
        private readonly IConfiguration _config;

        public TSharpContext(IConfiguration config) : base()
        {
            _config = config;
        }

        public DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                _config.GetConnectionString(""TSharpContext""));
            }
        }
    }
}
";
        }


        public static string ExpectedBaseEntity = @"// Auto-generated code
namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    public partial class BaseEntity : IBaseEntity
    {
    }
}
";

        public static string ExpectedIBaseEntity = @"// Auto-generated code
namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    public partial interface IBaseEntity
    {
    }
}
";


        public static string ExpectedRepository = @"// Auto-generated code
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.API.Entities; 
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository; 
using TSharp.UnitOfWorkGenerator.API.Entities;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository;

public partial class Repository<T> : IRepository<T> where T : BaseEntity
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
    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
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
            return await query.OrderBy(orderBy).ToListAsync();
        }

        return await query.ToListAsync();
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
    public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
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

        return await query.FirstOrDefaultAsync();
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
";

        public static string ExpectedIRepository = @"// Auto-generated code
using System.Linq.Expressions;
using TSharp.UnitOfWorkGenerator.API.Entities; 
 using TSharp.UnitOfWorkGenerator.API.Entities;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{
    public interface IRepository
    {
    }

    public partial interface IRepository<T> : IRepository where T : IBaseEntity
    {
        #region asynchronous methods

        /// <summary>
        /// Gets the entity by id
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""id""></param>
        /// <param name=""cancellationToken""></param>
        /// <returns><typeparamref name=""T""/></returns>
        Task<T> GetAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name=""T""/>
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""orderBy"">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name=""cancellationToken""></param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns>IEnumerable of <typeparamref name=""T""/></returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name=""T""/>
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""orderBy"">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""cancellationToken""></param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns><typeparamref name=""T""/></returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name=""T"">The type to return</typeparam>
        /// <param name=""filter"">Filters a sequence of values based on a predicate.</param>
        /// <param name=""includeProperties"">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name=""T""/>)</param>
        /// <returns><typeparamref name=""T""/></returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds a new entity to database
        /// </summary>
        /// <typeparam name=""T"">The type of the entity to add</typeparam>
        /// <param name=""entity""></param>
        /// <param name=""cancellationToken""></param>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins tracking the given entities, and any other reachable entities that are
        /// not already being tracked, in the EntityState.Added"" state such that they will
        /// be inserted into the database when DbContext.SaveChanges() is called.
        /// </summary>
        /// <param name=""entities""></param>
        /// <param name=""cancellationToken""></param>
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
        /// <param name=""cancellationToken""></param>
        Task RemoveAsync(int id, CancellationToken cancellationToken = default);

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
        T Get(int id);

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
        void Remove(int id);

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
    }
}
";

        public static string ExpectedEmployeeEntity = @"// Auto-generated code
namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    public partial class Employee : BaseEntity
    {
    }
}
";

        public static string ExpectedUnitOfWOrk = @"// Auto-generated code
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository; 
using TSharp.UnitOfWorkGenerator.API.Entities; 
using TSharp.UnitOfWorkGenerator.API.Entities; 

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository 
{
    public partial class UnitOfWork : IUnitOfWork 
    { 
        private readonly TSharpContext _db; 
        public IEmployeeRepository Employee {get; private set;} 

        public UnitOfWork 
        ( 
            TSharpContext db, 
            IEmployeeRepository EmployeeRepository,
        ) 
        { 
            _db = db; 
            Employee = EmployeeRepository; 
        }  

        public virtual void Dispose()
        {
            _db.Dispose();
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    } 
}
";


        public static string ExpectedIUnitOfWork = @"// Auto-generated code
namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository 
{
    public partial interface IUnitOfWork : IDisposable 
    { 
        IEmployeeRepository Employee {get; }
        void Save();
        Task SaveAsync();
    } 
}
";

        public static string ExpectedIEmployeeRepository = @"// Auto-generated code
using TSharp.UnitOfWorkGenerator.API.Entities; 
 

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository 
{
    public partial interface IEmployeeRepository : IRepository<Employee> 
    { 
    } 
}
";


        public static string ExpectedEmployeeRepository = @"// Auto-generated code
using TSharp.UnitOfWorkGenerator.API.Entities; 
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository; 
using TSharp.UnitOfWorkGenerator.API.Entities; 

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository 
{
    public partial class EmployeeRepository : Repository<Employee>, IEmployeeRepository 
    { 
        private readonly TSharpContext _context; 
 
        public EmployeeRepository(TSharpContext db) : base(db) 
        { 
            _context = db; 
        } 
    }
}
";
    }
}