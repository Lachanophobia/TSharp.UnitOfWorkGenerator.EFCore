using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.Core.Utils;

namespace TSharp.UnitOfWorkGenerator.API.Entyties;
public class TSharpContext : DbContext
{
    private readonly IConfiguration _config;

    public TSharpContext(IConfiguration config) :base()
    {
        _config = config;
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Employee> Employee { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
            _config.GetConnectionString("TSharpContext"));
        }
    }

}
