using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.DataAccess.Entities;
using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.DataAccess.Repositories.Repository;

//[UoWOverrideRepository]
//public class CustomRepository<T> : Repository<T> where T : BaseEntity
//{
//    public TSharpContext _db { get; set; }
//    private DbSet<T> dbSet;
//    public CustomRepository(TSharpContext db) : base(db)
//    {
//        _db = db;
//        dbSet = _db.Set<T>();
//    }

//    /// <inheritdoc />
//    public override Task AddAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        entity.CreatedDate = DateTime.Now;

//        return base.AddAsync(entity, cancellationToken);
//    }
//}