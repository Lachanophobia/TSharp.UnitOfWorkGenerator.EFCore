using System.Text;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{

    internal static partial class BuildTemplate
    {
        public static string BuildRepoTemplate(this Template templateRepo)
        {
            var template = $@"// Auto-generated code
{templateRepo.UsingStatements} 

namespace {templateRepo.Namespace} 
{{
    public partial class {templateRepo.RepoName} : {templateRepo.CustomRepository}<{templateRepo.Entity}>, {templateRepo.IRepoName} 
    {{ 
        private readonly {templateRepo.DBContextName} _context; 
 
        public {templateRepo.RepoName}({templateRepo.DBContextName} db) : base(db) 
        {{ 
            _context = db; 
        }} 
    }}
}}
";

            return template;
        }

        public static string BuildIRepoTemplate(this Template templateIRepo)
        {
            var template = $@"// Auto-generated code
{templateIRepo.UsingStatements} 

namespace {templateIRepo.Namespace} 
{{
    public partial interface {templateIRepo.IRepoName} : {templateIRepo.CustomRepository}<{templateIRepo.Entity}> 
    {{ 
    }} 
}}
";
            return template;
        }
    }
}
