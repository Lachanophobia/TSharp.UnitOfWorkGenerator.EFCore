using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.API.Entities;


namespace TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;

public interface ICustomRepository<T> : IRepository<T> where T : IBaseEntity
{

}
