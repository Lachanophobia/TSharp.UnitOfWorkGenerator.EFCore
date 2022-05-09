using TSharp.UnitOfWorkGenerator.DataAccess.Entities;

namespace TSharp.UnitOfWorkGenerator.DataAccess.Repositories.IRepository
{
    public partial interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetPostsFromPartialClass(CancellationToken cancellationToken = default);
    }
}
