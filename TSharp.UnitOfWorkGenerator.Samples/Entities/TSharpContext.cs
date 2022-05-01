using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TSharp.UnitOfWorkGenerator.Samples.Entities;
public class TSharpContext : DbContext
{
    private readonly IConfiguration _config;

    public TSharpContext(IConfiguration config) : base()
    {
        _config = config;
    }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
            _config.GetConnectionString("TSharpContext"));
        }
    }

}
