using TSharp.UnitOfWorkGenerator.Core.Utils;

namespace TSharp.UnitOfWorkGenerator.Samples.Entities;
[GenerateRepository]
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}