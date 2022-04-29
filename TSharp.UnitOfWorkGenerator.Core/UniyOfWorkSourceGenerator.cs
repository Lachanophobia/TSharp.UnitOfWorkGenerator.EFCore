using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TSharp.UnitOfWorkGenerator.Core
{
    [Generator]
    public partial class UniyOfWorkSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;
            List<GenRepoNames> GenNames = new List<GenRepoNames>();

            var reposToBeAdded = syntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(c => c.ToString().StartsWith("[GenerateRepository")))
                .ToList();

            var settingsAsJson = context.AdditionalFiles.FirstOrDefault().GetText().ToString();
            UoWSourceGenerator settings = JsonConvert.DeserializeObject<AppSettings>(settingsAsJson).UoWSourceGenerator;

            List<string> defaultRepoUsings = new List<string>() {
                $"using {settings.DBEntitiesNamespace}; \n",
                $"using {settings.IGenRepoNamespace}; \n"
            };

            List<string> defaultIRepoUsings = new List<string>()
            {
                $"using {settings.DBEntitiesNamespace}; \n"
            };

            List<string> defaultIUnitOfWorkUsings = new List<string>() {
                $"using {settings.IGenRepoNamespace}; \n"
            };

            //var reposUsingDirectives = reposToBeAdded.SelectMany(x => x.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Select(x => x.ToString()).Distinct();

            foreach (var repo in reposToBeAdded)
            {
                var className = repo.Identifier.ToString();
                var genClassName = $"{className}Repository";
                var genInterfaceName = $"I{className}Repository";

                var genRepoNames = new GenRepoNames()
                {
                    ClassName = className,
                    GenClassName = genClassName,
                    GenInterfaceName = genInterfaceName
                };

                GenNames.Add(genRepoNames);

                GenerateIRepo(genRepoNames, defaultIRepoUsings, settings,  context);
                GenerateRepo(genRepoNames, defaultRepoUsings, settings, context);
            }

            GenerateIUnitOfWork(GenNames, defaultIUnitOfWorkUsings, settings, context);
            GenerateUnitOfWork(GenNames, defaultIUnitOfWorkUsings, settings, context);
        }

        private void GenerateUnitOfWork(List<GenRepoNames> Repositories, List<string> defaultIUnitOfWorkUsings, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var UnitOfWorkBuilder = new StringBuilder();
            UnitOfWorkBuilder.Append("// Auto-generated code \n");

            foreach (var usingDirective in defaultIUnitOfWorkUsings)
            {
                UnitOfWorkBuilder.Append(usingDirective);
            }

            UnitOfWorkBuilder.Append($"namespace {settings.GenRepoNamespace} \n");
            UnitOfWorkBuilder.Append("{ \n");
            UnitOfWorkBuilder.Append("    public partial class UnitOfWork : IUnitOfWork \n");
            UnitOfWorkBuilder.Append("    { \n");

            // Constractor parameters
            UnitOfWorkBuilder.Append("       public UnitOfWork \n");
            UnitOfWorkBuilder.Append("       ( \n");

            foreach (var (repository, index) in Repositories.WithIndex())
            {
                UnitOfWorkBuilder.Append($"       {repository.GenInterfaceName} {repository.GenClassName}{(index != Repositories.Count - 1 ? "," : string.Empty)} \n");
            }

            UnitOfWorkBuilder.Append("       ) \n");

            // \\End Of - Constractor parameters

            // Constractor body
            UnitOfWorkBuilder.Append("       { \n");

            foreach (var repository in Repositories)
            {
                UnitOfWorkBuilder.Append($"       {repository.ClassName} = {repository.GenClassName}; \n");
            }

            UnitOfWorkBuilder.Append("       } \n");
            // \\End Of - Constractor body

            // Properties
            foreach (var repo in Repositories)
            {

                UnitOfWorkBuilder.Append($"        public {repo.GenInterfaceName} {repo.ClassName} " + "{get; private set;} \n");
            }
            UnitOfWorkBuilder.Append("    } \n");
            // \\End Of - Properties

            UnitOfWorkBuilder.Append("}");

            context.AddSource($"UnitOfWork.g.cs", UnitOfWorkBuilder.ToString());
        }

        private void GenerateIUnitOfWork(List<GenRepoNames> Repositories, List<string> defaultIUnitOfWorkUsings, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var IUnitOfWorkBuilder = new StringBuilder();
            IUnitOfWorkBuilder.Append("// Auto-generated code \n");

            foreach (var usingDirective in defaultIUnitOfWorkUsings)
            {
                IUnitOfWorkBuilder.Append(usingDirective);
            }

            IUnitOfWorkBuilder.Append($"namespace {settings.IGenRepoNamespace} \n");
            IUnitOfWorkBuilder.Append("{ \n");
            IUnitOfWorkBuilder.Append("    public partial interface IUnitOfWork \n");
            IUnitOfWorkBuilder.Append("    { \n");

            foreach (var repo in Repositories)
            {
                IUnitOfWorkBuilder.Append($"        {repo.GenInterfaceName} {repo.ClassName} " + "{get; } \n");
            }

            IUnitOfWorkBuilder.Append("    } \n");
            IUnitOfWorkBuilder.Append("}");

            context.AddSource($"IUnitOfWork.g.cs", IUnitOfWorkBuilder.ToString());

        }

        private void GenerateRepo(GenRepoNames RepoNames, List<string> defaultRepoUsings, UoWSourceGenerator settings, GeneratorExecutionContext Context)
        {
            var RepoBuilder = new StringBuilder();
            RepoBuilder.Append("// Auto-generated code \n");

            foreach (var item in defaultRepoUsings)
            {
                RepoBuilder.Append(item);
            }

            RepoBuilder.Append($"namespace {settings.GenRepoNamespace} \n");
            RepoBuilder.Append("{ \n");
            RepoBuilder.Append($"    public partial class {RepoNames.GenClassName} : Repository<{RepoNames.ClassName}>, {RepoNames.GenInterfaceName} \n");
            RepoBuilder.Append("    { \n");

            RepoBuilder.Append("        private readonly TSharpContext _context; \n \n");

            RepoBuilder.Append($"        public {RepoNames.GenClassName}({settings.DBContextName} db) : base(db)" + " \n");
            RepoBuilder.Append("        { \n");
            RepoBuilder.Append("            _context = db; \n");
            RepoBuilder.Append("        } \n");

            RepoBuilder.Append("    } \n");
            RepoBuilder.Append("}");

            Context.AddSource($"{RepoNames.GenClassName}.g.cs", RepoBuilder.ToString());
        }

        private void GenerateIRepo(GenRepoNames RepoNames, List<string> defaultIRepoUsings, UoWSourceGenerator settings, GeneratorExecutionContext Context)
        {
            var IRepoBuilder = new StringBuilder();
            IRepoBuilder.Append("// Auto-generated code \n");

            foreach (var item in defaultIRepoUsings)
            {
                IRepoBuilder.Append(item);
            }

            IRepoBuilder.Append($"namespace {settings.IGenRepoNamespace} \n");
            IRepoBuilder.Append("{ \n");
            IRepoBuilder.Append($"    public partial interface {RepoNames.GenInterfaceName} : IRepository<{RepoNames.ClassName}> \n");
            IRepoBuilder.Append("    { \n");

            IRepoBuilder.Append("    } \n");
            IRepoBuilder.Append("}");

            Context.AddSource($"{RepoNames.GenInterfaceName}.g.cs", IRepoBuilder.ToString());
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
//            Debug.WriteLine("Initalize code generator");
        }
    }
}