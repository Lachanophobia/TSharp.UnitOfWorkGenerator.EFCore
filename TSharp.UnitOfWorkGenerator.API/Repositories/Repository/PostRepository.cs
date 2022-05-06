using TSharp.UnitOfWorkGenerator.API.Entities;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository
{
    public partial class PostRepository : CustomRepository<Post>, IPostRepository
    {
        public async Task<List<Post>> GetPostsFromPartialClass(CancellationToken cancellationToken = default)
        {
            var posts = (await this.GetAllAsync(cancellationToken: cancellationToken)).ToList();
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
