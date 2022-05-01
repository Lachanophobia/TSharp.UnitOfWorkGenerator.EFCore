using TSharp.UnitOfWorkGenerator.Samples.Entities;

namespace TSharp.UnitOfWorkGenerator.Samples.Repositories.IRepository
{
    public partial interface IUnitOfWork : IDisposable
    {
        IPostRepository Post { get; }
        ISP_Call SP_Call { get; }
        void Save();
        Task SaveAsync();
    }
}
