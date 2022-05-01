using TSharp.UnitOfWorkGenerator.Samples.Entities;
using TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.Repository
{
    public partial class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly TSharpContext _db;
        public PostRepository(TSharpContext db) : base(db)
        {
            _db = db;
        }

    }
}
