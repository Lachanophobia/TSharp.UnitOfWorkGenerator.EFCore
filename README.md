# UnitOfWorkGenerator - Repository Pattern Made easy!

**UnitOfWorkGenerator** is a source generator. It runs during compilation, inspects your program for your dbContext Entities and produces the Repositories, the IRepositories, the UnitOfWork and the IUnitOfWork which are injected to your dll of the project you have installed the **UnitOfWorkGenerator**.

The goal of this project remove the boilerplate of the Repository Pattern, just by adding one attribute to you dbEntity and build your project all the repository files will be generated. **Just one attribute!!!**

Now you don't have to maintain large classes like the UnitOfWork every time you want to add a new Repository and you don't have to create the appropriate Repository files when you just want to use only the generic methods for an entity.   

All classes are created as **partial** classes so you have the ability to extended them.  

***supports .Net 5 and higher versions.***

# Installation 

1.  First you need to Install Entity Framework Core and create at least one dbEntity
2.  Install package //TODO 

## Settings

3. You need to specify the namespaces the UnitOfWorkGenerator will use to generate the files.
Into the root of your appsettings of your project add the following settings.  <br> **(Note: these settings need to exist on your main appsettings file and not into any other build configuration)**



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


## Build

3. Build the project :D 

## Dependency Injection
4. As you would do normally, you need to use DI to register the interfaces. I would recommend to use [Scrutor](https://github.com/khellang/Scrutor). With Scrutor you can forget the service registration for your repositories.
**See example:** 

    builder.Services.Scan(scan => scan
        .FromAssemblyOf<PostRepository>() // **will scan the Assembly where the PostRepository lives, just add any Repository Class**
        .AddClasses(classes => classes.AssignableTo<IRepository>()) // **Here leave the IRepository**
        .AsImplementedInterfaces()
        .WithScopedLifetime());
    
    builder.Services.AddScoped<ISP_Call, SP_Call>(); // **Add this only if you enable the ISP_Call**
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
