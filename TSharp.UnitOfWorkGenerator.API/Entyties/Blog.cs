
using TSharp.UnitOfWorkGenerator.Core.Utils;

namespace TSharp.UnitOfWorkGenerator.API.Entyties;

[GenerateRepository]
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}
