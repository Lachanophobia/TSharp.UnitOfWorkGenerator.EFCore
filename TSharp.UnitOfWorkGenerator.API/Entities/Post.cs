using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.API.Entities;
[GenerateRepository]
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}