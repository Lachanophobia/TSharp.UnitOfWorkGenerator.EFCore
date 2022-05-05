using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.Samples.Entities;
using TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.Repository;

public class CustomRepository<T> : Repository<T> where T : BaseEntity
{
    public TSharpContext _db { get; set; }
    private DbSet<T> dbSet;
    public CustomRepository(TSharpContext db) : base(db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    /// <inheritdoc />
    public override void Add(T entity)
    {
        entity.CreatedDate = DateTime.Now;

        base.Add(entity);
    }
}
