using System;
using System.Collections.Generic;
using System.Text;
using TSharp.UnitOfWorkGenerator.Samples.Entities;
using TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.Repository
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly TSharpContext _db;
        public IPostRepository Post { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public UnitOfWork(TSharpContext db, IPostRepository post, ISP_Call sP_Call)
        {
            _db = db;
            Post = post;
            SP_Call = sP_Call;
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