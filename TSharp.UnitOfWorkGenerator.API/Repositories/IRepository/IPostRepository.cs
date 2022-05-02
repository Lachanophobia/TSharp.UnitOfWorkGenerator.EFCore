using TSharp.UnitOfWorkGenerator.API.Entities;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{
    public partial interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetPostsFromPartialClass(CancellationToken cancellationToken = default);
    }
}
