using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.API.Entyties;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly TSharpContext _db;
    internal DbSet<T> dbSet;

    public Repository(TSharpContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }


    public async Task<T> GetAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }


        return await query.FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(int id)
    {
        T entity = await dbSet.FindAsync(id);
        await RemoveAsync(entity);
    }

    public async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

}
