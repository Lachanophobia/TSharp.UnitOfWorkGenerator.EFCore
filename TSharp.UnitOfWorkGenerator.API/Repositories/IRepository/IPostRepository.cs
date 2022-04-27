using TSharp.UnitOfWorkGenerator.API.Entyties;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{
    public partial interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetPostsFromPartialClass();
    }
}
