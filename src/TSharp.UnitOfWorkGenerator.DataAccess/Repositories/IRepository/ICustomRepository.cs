using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.DataAccess.Entities;


namespace TSharp.UnitOfWorkGenerator.DataAccess.Repositories.IRepository;

public interface ICustomRepository<T> : IRepository<T> where T : IBaseEntity
{

}
