using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TSharp.UnitOfWorkGenerator.EFCore.Models;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TSharp.UnitOfWorkGenerator.EFCore.Helpers;

namespace TSharp.UnitOfWorkGenerator.EFCore
{
    [Generator]
    public partial class UnitOfWorkSourceGenerator : ISourceGenerator
    {
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

        public void Execute(GeneratorExecutionContext context)
        {
            if (!Diagnostics.CheckedForEntityFrameworkCoreDependency(context))
                return;

            var syntaxTrees = context.Compilation.SyntaxTrees;

            var relevantDeclarationSyntaxes = syntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(c => c.ToString().StartsWith("[UoW")))
                .ToList();

            var reposToBeAdded = relevantDeclarationSyntaxes
                .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[UoWGenerateRepository]"))).ToList();

            var customRepository = relevantDeclarationSyntaxes
                .FirstOrDefault(x => x.AttributeLists.Any(c => c.ToString().Equals("[UoWOverrideRepository]")));

            var (dbContextTypeDeclaration, dbContextName, dbContextNamespace) = SourceGenHelper.GetDbContextInfo(context, relevantDeclarationSyntaxes);

            if (dbContextTypeDeclaration == null)
                return;

            if (!Diagnostics.ValidateReposToBeAdded(context, reposToBeAdded))
                return;

            var settings = SourceGenHelper.GetUoWSourceGeneratorSettings(context, dbContextName, dbContextNamespace);

            Generate.BaseEntity(settings, context);
            Generate.IBaseEntity(settings, context);
            Generate.BaseIRepository(settings, context);
            Generate.BaseRepository(settings, context);

            if (settings.EnableISP_Call)
            {
                if (!Diagnostics.CheckedForDapperDependency(context))
                    return;

                Generate.ISP_Call(settings, context);
                Generate.SP_Call(settings, context);
            }

            StringBuilder uoWConstructor = new(), uoWParameters = new(), uoWProperties = new(), iUoWProperties = new();

            Populate.UnitOfWorkConstInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, settings);

            var toBeAddedLast = reposToBeAdded.LastOrDefault();
            reposToBeAdded.Remove(toBeAddedLast);

            var options = new ParallelOptions { MaxDegreeOfParallelism = 4 };

            if (reposToBeAdded.Any())
            {
                ConcurrentBag<TypeDeclarationSyntax> repos = new(reposToBeAdded);

                Parallel.ForEach(repos, options, repo =>
                {
                    var entity = repo.Identifier.ToString();

                    var genRepoNames = new GeneratedRepoNames(entity);

                    Generate.dbEntity(genRepoNames, settings, context);
                    Generate.IRepository(genRepoNames, settings, context, customRepository);
                    Generate.Repository(genRepoNames, settings, context, customRepository);

                    Populate.UnitOfWorkGeneratedInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, genRepoNames, false);
                });
            }

            var lastRepo = toBeAddedLast.Identifier.ToString();

            var genRepoNames = new GeneratedRepoNames(lastRepo);

            Generate.dbEntity(genRepoNames, settings, context);
            Generate.IRepository(genRepoNames, settings, context, customRepository);
            Generate.Repository(genRepoNames, settings, context, customRepository);

            Populate.UnitOfWorkGeneratedInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, genRepoNames, true);

            var generatedUoWInfo = SourceGenHelper.GetGeneratedUoWInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties);

            Generate.IUnitOfWork(generatedUoWInfo, settings, context);
            Generate.UnitOfWork(generatedUoWInfo, settings, context);
        }
    }
}
