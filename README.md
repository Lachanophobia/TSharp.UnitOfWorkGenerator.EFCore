# UnitOfWorkGenerator - Repository Pattern Made easy!

**UnitOfWorkGenerator** is a source generator. It runs during compilation, inspects your program for your dbContext Entities and produces the Repositories, the IRepositories, the UnitOfWork and the IUnitOfWork which are injected to your dll of the project you have installed the **UnitOfWorkGenerator**.

The goal of this project is to remove the boilerplate of the Repository Pattern, just by adding one attribute to your dbEntity and build your project all the repository files will be generated. **Just one attribute!!!**

Now you don't have to maintain large classes like the UnitOfWork every time you want to add a new Repository and you don't have to create the appropriate Repository files when you just want to use only the generic methods for an entity.   

- Large data bases with many tables? Hassle to maintain or create classes all the time? - Not a problem anymore!
- Small projects where you are thinking to not apply the pattern to save time? - Not a problem anymore!
- Worry about performance penalties when stuff are generated? - Not a problem, all are done at compilation time!

All classes are created as **partial** classes so you have the ability to extended them.  

***supports .Net 5 and higher versions.***

# Installation 

1.  First you need to Install Entity Framework Core and create at least one dbEntity
2.  Install package //TODO 

## Settings

3. You need to specify the namespaces the UnitOfWorkGenerator will use to generate the files.
Into the root of your appsettings of your project add the following settings.  <br> **(Note: these settings need to exist on your main appsettings file and not into any other build configuration)**

I



    "UoWSourceGenerator": {
    	"IRepoNamespace": "TSharp.UnitOfWorkGenerator.API.Repositories.IRepository",
    	"RepoNamespace": "TSharp.UnitOfWorkGenerator.API.Repositories.Repository",
        "DBEntitiesNamespace": "TSharp.UnitOfWorkGenerator.API.Entities",
        "DBContextName": "TSharpContext",
        "EnableISP_Call": "True",
        "EnableGuidIdentityColumn": "False"
    }

| Settings                |Type      |Description                  |                    
|-------------------------|----------|-----------------------------|
|IRepoNamespace           |`string` (required)|The Namespace where you want to generate the Interfaces|
|RepoNamespace            |`string` (required)|The Namespace where you want to generate the Implementation Classes|
|DBEntitiesNamespace      |`string` (required)|The Namespace where your dbEntities live|
|DBContextName            |`string` (required)|The Name of your class where you inherit from the DbContext|
|EnableISP_Call           |`bool` (optional)  |This will add an extra Interface and Class into UnitOfWork to execute stored procedures. You need to **install Dapper** for this feature. Default is **false**|
|EnableGuidIdentityColumn |`bool` (optional)  |Simply Choose between int and Guid as IdentityColumn. Default is **false**|

## Generate the Repositories
Just add this attribute `[GenerateRepository]` to an entity

	using TSharp.UnitOfWorkGenerator.Core.Utils;
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


## Build

3. Build the project :D 

## Dependency Injection
4. As you would do normally, you need to use DI to register the interfaces. I would recommend to use [Scrutor](https://github.com/khellang/Scrutor). With Scrutor you can forget the service registration for your repositories.
**See example:** <br>

		builder.Services.Scan(scan => scan
            .FromAssemblyOf<PostRepository>() // **Will scan the Assembly where the PostRepository lives, just add any Repository Class**
            .AddClasses(classes => classes.AssignableTo<IRepository>()) // **Here leave the IRepository**
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        builder.Services.AddScoped<ISP_Call, SP_Call>(); // **Add this only if you enable the ISP_Call**
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

# Exposed Methods
## IRepository 

	using System.Linq.Expressions;
    public partial interface IRepository<T> : IRepository where T : class
    {

        Task<T> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties);

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

## ISP_Call
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

## IUnitOfWork
    public partial interface IUnitOfWork : IDisposable
    {
        IPostRepository Post { get; }
        ISP_Call SP_Call { get; }
        void Save();
        Task SaveAsync();
    }
