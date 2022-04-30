namespace TSharp.UnitOfWorkGenerator.Core.Templates
{
    internal class UoWTemplate
    {
        private string UsingStatements { get; set; }
        private string Namespace { get; set; }
        private string IRepoName { get; set; }
        private string RepoName { get; set; }
        private string Entity { get; set; }
        private string DBContextName { get; set; }
        private string Properties { get; set; }
        private string Parameters { get; set; }
        private string Constructor { get; set; }

        public UoWTemplate WithProperties(string properties)
        {
            this.Properties = properties;

            return this;
        }

        public UoWTemplate WithParameters(string parameters)
        {
            this.Parameters = parameters;

            return this;
        }

        public UoWTemplate WithConstructor(string constructor)
        {
            this.Constructor = constructor;

            return this;
        }

        public UoWTemplate WithUsingStatements(string usingStatements)
        {
            this.UsingStatements = usingStatements;

            return this;
        }

        public UoWTemplate WithNamespace(string @namespace)
        {
            this.Namespace = @namespace;

            return this;
        }

        public UoWTemplate WithRepoName(string repoName)
        {
            this.RepoName = repoName;

            return this;
        }

        public UoWTemplate WithEntity(string entity)
        {
            this.Entity = entity;

            return this;
        }

        public UoWTemplate WithDBContextName(string dBContextName)
        {
            this.DBContextName = dBContextName;

            return this;
        }

        public UoWTemplate WithIRepoName(string iRepoName)
        {
            this.IRepoName = iRepoName;

            return this;
        }

        public string BuildRepoTemplate()
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

            var repoTemplate = string.Format(template, this.UsingStatements, this.Namespace, this.RepoName, this.Entity, this.IRepoName, this.DBContextName);

            return repoTemplate;
        }

        public string BuildIRepoTemplate()
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

            var iRepoTemplate = string.Format(template, this.UsingStatements, this.Namespace, this.IRepoName, this.Entity);

            return iRepoTemplate;
        }

        public string BuildUoWTemplate()
        {
            var template = @"// Auto-generated code 
{0} 

namespace {1} 
{{
    public partial class UnitOfWork : IUnitOfWork 
    {{ 
{2}

       public UnitOfWork 
       ( 
{3} 
       ) 
       {{ 
{4} 
       }}  
    }} 
}}
";
            var uoWTemplate = string.Format(template, this.UsingStatements, this.Namespace, this.Properties, this.Parameters, this.Constructor);

            return uoWTemplate;
        }

        public string BuildIUoWTemplate()
        {
            var template = @"// Auto-generated code 
{0} 

namespace {1} 
{{
    public partial interface IUnitOfWork 
    {{ 
{2} 
    }} 
}}
";

            var iUoWTemplate = string.Format(template, this.UsingStatements, this.Namespace, this.Properties);

            return iUoWTemplate;
        }
    }
}
