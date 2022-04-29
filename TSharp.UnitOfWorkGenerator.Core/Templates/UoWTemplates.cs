namespace TSharp.UnitOfWorkGenerator.Core.Templates
{
    internal static class UoWTemplates
    {
        public static string GetRepoTemplate()
        {
            var repoTemplate = $@"// Auto-generated code 
{{UsingStatements}} 

namespace {{Namespace}} 
{{
    public partial class {{RepoName}} : Repository<{{Entity}}>, {{Interface}} 
    {{ 
        private readonly {{DBContextName}} _context; 
 
        public {{RepoName}}({{DBContextName}} db) : base(db) 
        {{ 
            _context = db; 
        }} 
    }}
}}
";
            return repoTemplate;
        }

        public static string GetIRepoTemplate()
        {
            var iRepoTemplate = $@"// Auto-generated code 
{{UsingStatements}} 

namespace {{Namespace}} 
{{
    public partial interface {{IRepoName}} : IRepository<{{Entity}}> 
    {{ 
    }} 
}}
";
            return iRepoTemplate;
        }

        public static string GetUoWTemplate()
        {
            var uoWTemplate = $@"// Auto-generated code 
{{UsingStatements}} 

namespace {{Namespace}} 
{{
    public partial class UnitOfWork : IUnitOfWork 
    {{ 
{{Properties}}

       public UnitOfWork 
       ( 
{{ConstructorParameters}} 
       ) 
       {{ 
{{Constructor}} 
       }}  
    }} 
}}
";
            return uoWTemplate;
        }

        public static string GetIUoWTemplate()
        {
            var iUoWTemplate = $@"// Auto-generated code 
{{UsingStatements}} 

namespace {{Namespace}} 
{{
    public partial interface IUnitOfWork 
    {{ 
{{Properties}} 
    }} 
}}
";
            return iUoWTemplate;
        }
    }
}
