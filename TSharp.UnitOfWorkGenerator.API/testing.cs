//// Auto-generated code 
//using GeneratedUnitOfWork;
//using TSharp.UnitOfWorkGenerator.API.Entyties;
//using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;
//namespace GeneratedUnitOfWorkk
//{
//    public partial class UnitOfWorkk : IUnitOfWork
//    {
//        public UnitOfWorkk
//        (
//        IEmployeeRepository EmployeeRepository,
//        IBlogRepository BlogRepository,
//        IPostRepository PostRepository
//        )
//        {
//            Employee = EmployeeRepository;
//            Blog = BlogRepository;
//            Post = PostRepository;
//        }
//        public IEmployeeRepository Employee { get; private set; }
//        public TSharp.UnitOfWorkGenerator.API.Repositories.Repository.EmployeeRepository Employee2 { get; private set; }
//        public IBlogRepository Blog { get; private set; }
//        public IPostRepository Post { get; private set; }
//    }
//}

//// Auto-generated code 
//namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository
//{
//    public partial class EmployeeRepositoryy : Repository<Employee>, IEmployeeRepository
//    {
//        private readonly TSharpContext _context;
//        public EmployeeRepositoryy(TSharpContext db) : base(db)
//        {
//            _context = db;
//        }
//    }
//}


using TSharp.UnitOfWorkGenerator.API.Entyties;
using TSharp.UnitOfWorkGenerator.API.Repositories.IRepository;
namespace TSharp.UnitOfWorkGenerator.API.Repositories.Repository
{
    public partial class BlogRepositoryy : Repository<Blog>, IBlogRepository
    {
        private readonly TSharpContext _context;

        public BlogRepositoryy(TSharpContext db) : base(db)
        {
            _context = db;
        }
    }
}