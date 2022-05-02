using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{

    internal static partial class BuildTemplate
    {
        public static string BuildRepoTemplate(this Template templateRepo)
        {
            var template = @"// Auto-generated code
{0} 

namespace {1} 
{{
    public partial class {2} : Repository<{3}>, {4} 
    {{ 
        private readonly {5} _context; 
 
        public {2}({5} db) : base(db) 
        {{ 
            _context = db; 
        }} 
    }}
}}
";

            var repoTemplate = string.Format(template, templateRepo.UsingStatements, templateRepo.Namespace, templateRepo.RepoName,
                templateRepo.Entity, templateRepo.IRepoName, templateRepo.DBContextName);

            return repoTemplate;
        }

        public static string BuildIRepoTemplate(this Template templateIRepo)
        {
            var template = @"// Auto-generated code 
{0} 

namespace {1} 
{{
    public partial interface {2} : IRepository<{3}> 
    {{ 
    }} 
}}
";

            var iRepoTemplate = string.Format(template, templateIRepo.UsingStatements, templateIRepo.Namespace, templateIRepo.IRepoName, templateIRepo.Entity);

            return iRepoTemplate;
        }
    }
}
