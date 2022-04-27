using TSharp.UnitOfWorkGenerator.Core.Utils;

namespace TSharp.UnitOfWorkGenerator.API.Entyties;
[GenerateRepository]
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}