using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.API.Repositories.Repository;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;
using TSharp.UnitOfWorkGenerator.API.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddDbContext<TSharpContext>(options => {
    options.UseSqlServer(config.GetConnectionString("TSharpContext"));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<PostRepository>()
    .AddClasses(classes => classes.AssignableTo<IRepository>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<ISP_Call, SP_Call>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
