using TSharp.UnitOfWorkGenerator.API.Entyties;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository
{
    public partial class PostRepository : Repository<Post>, IPostRepository
    {
        public async Task<List<Post>> GetPostsFromPartialClass()
        {

            var posts = (await this.GetAllAsync()).ToList();
            posts.Add(new Post()
            {
                BlogId = 1,
                Title = "This post comes from a partial class",
                Content = "This post comes from a partial class",
                PostId = 3
            });

            return posts;
        }
    }
}
