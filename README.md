
# UnitOfWorkGenerator - Repository Pattern Made easy!

**UnitOfWorkGenerator** is a source generator. It runs during compilation, inspects your program for your dbContext Entities and produces the Repositories, the IRepositories, the UnitOfWork and the IUnitOfWork which are injected to your dll of the project you have installed the **UnitOfWorkGenerator**.

The goal of this project is to remove the boilerplate of the Repository Pattern, just by adding one attribute to your dbEntity and build your project all the repository files will be generated. **Just one attribute!!!**

Now you don't have to maintain large classes like the UnitOfWork every time you want to add a new Repository and you don't have to create the appropriate Repository files when you just want to use only the generic methods for an entity.   

- Large data bases with many tables? Hassle to maintain or create classes all the time? - Not a problem anymore!
- Small projects where you are thinking to not apply the pattern to save time? - Not a problem anymore!
- Worry about performance penalties when code is generated? - Not a problem, all are done at compilation time!

All classes are created as **partial** classes so you have the ability to extended them.  

***supports .Net 5 and higher versions.***

# Usage
Add the attribute [GenerateRepository] to your dbEntity, build the project and that's it! your repository has been created!

![Alt Text](https://media.giphy.com/media/MO4vOE6zV4TzISSa5u/giphy.gif)

# Installation 

**1.**  First you need to Install Entity Framework Core and create at least one dbEntity. Your db entities need to be created as **partial** classes. <br>
**2.**  Install-Package [TSharp.UnitOfWorkGenerator.EFCore](https://www.nuget.org/packages/TSharp.UnitOfWorkGenerator.EFCore/)<br>
**3.**  Install-Package [TSharp.UnitOfWorkGenerator.EFCore.Utils](https://www.nuget.org/packages/TSharp.UnitOfWorkGenerator.EFCore.Utils/)<br>
**4.** Decorate your dbContext class with the attribute `[UoWDefineDbContext]` and your db entities with this attribute `[UoWGenerateRepository]`. So your classes should look like this.

```csharp
    [UoWDefineDbContext]
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
```
```csharp
[UoWGenerateRepository]
public partial class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```
<hr>

 **5.**  Build your project, and that's it! your repositories have been created!

The default namespace for your repository interfaces will be : `{yourAssemblyName}.Repositories.IRepository`  <br> The default namespace for your repository classes will be : `{yourAssemblyName}.Repositories.IRepository`

You can view your generated classes in the project you have installed the UnitOfWorkGenerator under your Dependencies -> Analyzers -> TSharp.UnitOfWorkGenerator.EFCore

**Note: Some times Visual Studio can't resolve the newly created namespaces. It would be a good idea to restart Visual Studio. Later on when you will be consuming the UnitOfWorkGenerator, you will NOT have to restart Visual Studio all the time. <br><br>One more scenario that can occur is when you clean your solution and close the VS. So next time you will generate the repositories you will have to restart VS as well.**
<hr>

**6.** One last step before you start using the unit of work is to register all the generated repositories. I would recommend to use [Scrutor](https://github.com/khellang/Scrutor). With Scrutor you can forget the service registration for your repositories.
**See example:** <br>
```csharp
    builder.Services.Scan(scan => scan
           .FromAssemblyOf<PostRepository>() // **Will scan the Assembly where the PostRepository lives, just add any Repository Class**
           .AddClasses(classes => classes.AssignableTo<IRepository>()) // **Here leave the IRepository**
           .AsImplementedInterfaces()
           .WithScopedLifetime());
	    
    builder.Services.AddScoped<ISP_Call, SP_Call>(); // **Add this only if you enable the ISP_Call**
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

## You can start using it! 
```csharp
    public TestController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("GetPosts")]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
    {
        return Ok(await _unitOfWork.Post.GetFirstOrDefaultAsync(x => x.BlogId!= 1, cancellationToken));
    }
```
# Customization
**Here there is a lot of flexibility!**
## 1. Settings
You can create a json file in the root of your project (the one that used the UnitOfWorkGenerator) with this name `uow.config.json` and overwrite the repositories namespace, add additional generic methods to execute stored procedures and use Guids as Identity Column.
Below you can find the settings for the json file.
```json
    "UoWSourceGenerator": {
        "IRepoNamespace": "MyCustomNamespace.IRepositories",
        "RepoNamespace": "MyCustomNamespace.Repositories",
        "EnableISP_Call": "True",
        "EnableGuidIdentityColumn": "False"
    }
```

| Settings                |Type      |Description                  |                    
|-------------------------|----------|-----------------------------|
|IRepoNamespace           |`string` (required)|The Namespace where you want to generate the Interfaces|
|RepoNamespace            |`string` (required)|The Namespace where you want to generate the Implementation Classes|
|EnableISP_Call           |`bool` (optional)  |This will add an extra Interface and Class into UnitOfWork to execute stored procedures. You need to **install Dapper** for this feature. Default is **false**|
|EnableGuidIdentityColumn |`bool` (optional)  |Simply choose between int and Guid as IdentityColumn. Default is **false**|

Also you will need to add to your .csproj file of the same project, the following xml for the generator to pick up your settings. 
```csharp
<ItemGroup>
	<AdditionalFiles Include="uow.config.json"  />
</ItemGroup>
```
## 2. Overrides
**2.1.** All classes are partials and all methods are virtual. So you can extend your repositories and override the default methods! Just remember to use the generated namespaces of these you provided .

```csharp
using TSharp.UnitOfWorkGenerator.API.Entities;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{
    public partial interface IPostRepository
    {
        Task<List<Post>> GetPostsFromPartialClass(CancellationToken cancellationToken = default);
    }
}
```
<hr>

**2.2** Override the default generic repository!
You can create you own generic repository and decorated with this attribute `[UoWOverrideRepository]`. 

To do that you will need to inherit from the default `Repository` and also to provide and an interface for that, which again needs to implement the default `IRepository`. 

Your `ICustomRepository` needs to have a Constraint of type `IBaseEntity`
and your `CustomRepository` needs to have a Constraint of type `BaseEntity`

*The `BaseEntity` is the reason your dbEntities need to be partial classes. 
Alongside with the generated repositories, partial classes of your dbEntities are generated as well and they Inherit from the `BaseEntity` which also has an interface `IBaseEntity`*

**This is all you need to Override the default Repository!**
<hr>

Now, why you may need the BaseEntity??<br>
Because this gives you the flexibility to have some generics properties for all your dbEntities. 

A very common scenario is that you can have some properties like CreatedDate, UpdatedDate, CreatedBy, UpdateBy etc.
and you want to generalise the population of these. 

**Here is an example how you can achieve this:**
Create the IBaseEntity and the BaseEntity as partial classes with the namespace of your dbEntites and add the generic properties you wish.
 
```csharp
namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    public partial class BaseEntity : IBaseEntity
    {
        public DateTime CreatedDate { get; set; }

    }
}

```
```csharp
namespace TSharp.UnitOfWorkGenerator.API.Entities
{
    public partial interface IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
    }
}

```

Then create your custom `ICustomRepository` and `CustomRepository` 

```csharp
namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository
{

    public interface ICustomRepository<T> : IRepository<T> where T : IBaseEntity
    {

    }
}
```
And override the methods you wish or create new ones!  
```csharp
[UoWOverrideRepository]
public class CustomRepository<T> : Repository<T> where T : BaseEntity
{
    public TSharpContext _db { get; set; }
    public CustomRepository(TSharpContext db) : base(db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public override Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedDate = DateTime.Now;

        return base.AddAsync(entity, cancellationToken);
    }
}
```
If you used [Scrutor](https://github.com/khellang/Scrutor), you don't have to change anything in the service registration!

**Basically, you can customize everything!** 

# Exposed Methods
## IRepository 

```csharp
    using System.Linq.Expressions;

    public partial interface IRepository<T> : IRepository where T : class
    {
        Task<T> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entity);

        Task RemoveAsync(int id, CancellationToken cancellationToken = default);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entity);

        T Get(int id);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entity);

        void Remove(int id);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);

        void AddRange(IEnumerable<T> entities);
    }
```
## ISP_Call
```csharp

    public partial interface ISP_Call : IDisposable
    {
        void Dispose();
       
        void Execute(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
               
        Task ExecuteAsync(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);
        
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
       
        Task<IEnumerable<T>> ListAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);
        
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);

        Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);
        
        T OneRecord<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
        
        Task<T> OneRecordAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null, CancellationToken cancellationToken = default);
        
        T Single<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
        
        Task<T> SingleAsync<T>(string procedureName, DynamicParameters param = null, IDbConnection? connection = null, IDbTransaction? transaction = null, int? commandTimeout = null);
    }
```
## IUnitOfWork

```csharp
    public partial interface IUnitOfWork : IDisposable
    {
        IPostRepository Post { get; }
        ISP_Call SP_Call { get; }
        void Save();
        Task SaveAsync();
    }
```
# Debugging
You can view and dedug the generated files under your Dependencies -> Analyzers -> TSharp.UnitOfWorkGenerator.EFCore

![Alt Text](https://media.giphy.com/media/lnn1mBfmq15mV4yvT1/giphy.gif)

# Extend


```

```csharp
using TSharp.UnitOfWorkGenerator.API.Entities;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository
{
    public partial class PostRepository : Repository<Post>, IPostRepository
    {
        public async Task<List<Post>> GetPostsFromPartialClass(CancellationToken cancellationToken = default)
        {
            var posts = (await this.GetAllAsync(cancellationToken: cancellationToken)).ToList();
            posts.Add(new Post()
            {
                BlogId = 1,
                Title = "My New Method",
                Content = "My New Method",
                PostId = 3
            });

            return posts;
        }

        /// <inheritdoc />
        public override Post Get(int id)
        {
            return new Post()
            {
                BlogId = 1,
                Title = "override Get",
                Content = "override Get",
                PostId = 3
            };
        }
    }
}

```
Keep coding! Please get in touch for any issues or questions!

 
