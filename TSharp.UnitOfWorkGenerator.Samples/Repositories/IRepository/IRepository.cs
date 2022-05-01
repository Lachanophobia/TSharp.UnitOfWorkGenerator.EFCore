using System.Linq.Expressions;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository
{
    public interface IRepository
    {
    }

    public partial interface IRepository<T> : IRepository where T : class
    {
        #region asynchronous methods

        /// <summary>
        /// Gets the entity by id
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="id"></param>
        /// <returns><typeparamref name="T"/></returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="filter">Filters a sequence of values based on a predicate.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name="includeProperties">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name="T"/>)</param>
        /// <returns>IEnumerable of <typeparamref name="T"/></returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="filter">Filters a sequence of values based on a predicate.</param>
        /// <param name="includeProperties">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name="T"/>)</param>
        /// <returns><typeparamref name="T"/></returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds a new entity to database
        /// </summary>
        /// <typeparam name="T">The type of the entity to add</typeparam>
        /// <param name="entity"></param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to update</typeparam>
        /// <param name="entity"></param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable entities to update</typeparam>
        /// <param name="entity"></param>
        Task UpdateRangeAsync(IEnumerable<T> entity);

        /// <summary>
        /// Deletes the entity by its id
        /// </summary>
        /// <param name="id"></param>
        Task RemoveAsync(int id);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <typeparam name="T">The type to delete</typeparam>
        /// <param name="entity"></param>
        Task RemoveAsync(T entity);

        /// <summary>
        /// Deletes a range of entities
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable entities to delete</typeparam>
        /// <param name="entity"></param>
        Task RemoveRangeAsync(IEnumerable<T> entity);

        #endregion

        #region synchronous methods

        /// <summary>
        /// Gets the entity by id
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="id"></param>
        /// <returns><typeparamref name="T"/></returns>
        T Get(int id);

        /// <summary>
        /// Gets all entities as IEnumerable of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="filter">Filters a sequence of values based on a predicate.</param>
        /// <param name="orderBy">Sorts the elements of a sequence in ascending order according to a key.</param>
        /// <param name="includeProperties">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name="T"/>)</param>
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Gets the first or default
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="filter">Filters a sequence of values based on a predicate.</param>
        /// <param name="includeProperties">Specifies related entities to include in the query results. The navigation property 
        /// to be included is specified starting with the type of entity being queried (<typeparamref name="T"/>)</param>
        /// <returns><typeparamref name="T"/></returns>
        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Adds a new entity to database
        /// </summary>
        /// <typeparam name="T">The type of the entity to add</typeparam>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <typeparam name="T">The type of the entity to update</typeparam>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable entities to update</typeparam>
        /// <param name="entity"></param>
        void UpdateRange(IEnumerable<T> entity);


        /// <summary>
        /// Deletes the entity by its id
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        /// Deletes the entity
        /// </summary>
        /// <typeparam name="T">The type to delete</typeparam>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Deletes a range of entities
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable entities to delete</typeparam>
        /// <param name="entity"></param>
        void RemoveRange(IEnumerable<T> entity);

        #endregion
    }
}