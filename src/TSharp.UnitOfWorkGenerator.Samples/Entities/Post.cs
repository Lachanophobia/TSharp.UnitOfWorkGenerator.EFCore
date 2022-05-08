using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.Samples.Entities;
[UoWGenerateRepository]
public class Post : BaseEntity
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}