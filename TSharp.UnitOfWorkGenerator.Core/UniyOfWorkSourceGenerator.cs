using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TSharp.UnitOfWorkGenerator.Core.Models;
using TSharp.UnitOfWorkGenerator.Core.Templates;

namespace TSharp.UnitOfWorkGenerator.Core
{
    [Generator]
    public partial class UniyOfWorkSourceGenerator : ISourceGenerator
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
            var syntaxTrees = context.Compilation.SyntaxTrees;
            var genRepoNamesList = new List<GeneratedRepoNames>();

            var reposToBeAdded = syntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[GenerateRepository]", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            //var reposUsingDirectives = reposToBeAdded.SelectMany(x => x.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Select(x => x.ToString()).Distinct();

            var settingsAsJson = context.AdditionalFiles.FirstOrDefault().GetText().ToString();
            var settings = JsonConvert.DeserializeObject<AppSettings>(settingsAsJson).UoWSourceGenerator;

            GenerateBaseIRepo(settings, context);
            GenerateBaseRepo(settings, context);
            GenerateISP_Call(settings, context);
            GenerateSP_Call(settings, context);

            foreach (var repo in reposToBeAdded)
            {
                var entity = repo.Identifier.ToString();

                var genRepoNames = new GeneratedRepoNames()
                {
                    Entity = entity,
                    RepoName = $"{entity}Repository",
                    IRepoName = $"I{entity}Repository"
                };

                genRepoNamesList.Add(genRepoNames);

                GenerateIRepo(genRepoNames, settings, context);
                GenerateRepo(genRepoNames, settings, context);
            }

            var generatedUoWInfo = GetGeneratedUoWInfo(genRepoNamesList, settings);

            GenerateIUoW(generatedUoWInfo, settings, context);
            GenerateUoW(generatedUoWInfo, settings, context);
        }

        #region Generate Source Code

        private void GenerateISP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace
            }.BuildISP_CallTemplate();

            context.AddSource("ISP_Call.g.cs", template);
        }

        private void GenerateSP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.DBEntitiesNamespace}; \n" +
                $"using {settings.IRepoNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                DBContextName = settings.DBContextName,
            }.BuildSP_CallTemplate();

            context.AddSource("SP_Call.g.cs", template);
        }

        private void GenerateBaseRepo(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings = 
                $"using {settings.DBEntitiesNamespace}; \n" +
                $"using {settings.IRepoNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                DBContextName = settings.DBContextName,
                IdentityColumn = settings.EnableGuidIdentityColumn ? "Guid" : "int"
            }.BuildBaseRepoTemplate();

            context.AddSource($"Repository.g.cs", template);
        }

        private void GenerateBaseIRepo(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace,
                IdentityColumn = settings.EnableGuidIdentityColumn ? "Guid" : "int"
            }.BuildIBaseRepoTemplate();

            context.AddSource($"IRepository.g.cs", template);
        }

        private void GenerateUoW(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.IRepoNamespace}; \n" +
                $"using {settings.DBEntitiesNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                Properties = generatedInfo.UoW_Properties,
                Parameters = generatedInfo.UoW_Parameters,
                Constructor = generatedInfo.UoW_Constructor
            }.BuildUoWTemplate();

            context.AddSource($"UnitOfWork.g.cs", template);
        }

        private void GenerateIUoW(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace,
                Properties = generatedInfo.IUoW_Properties,
            }.BuildIUoWTemplate();

            context.AddSource($"IUnitOfWork.g.cs", template);
        }

        private void GenerateRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
               $"using {settings.DBEntitiesNamespace}; \n" +
               $"using {settings.IRepoNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                RepoName = genRepoNames.RepoName,
                Entity = genRepoNames.Entity,
                IRepoName = genRepoNames.IRepoName,
                DBContextName = settings.DBContextName
            }.BuildRepoTemplate();

            context.AddSource($"{genRepoNames.RepoName}.g.cs", template);
        }

        private void GenerateIRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
              $"using {settings.DBEntitiesNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.IRepoNamespace,
                Entity = genRepoNames.Entity,
                IRepoName = genRepoNames.IRepoName,
            }.BuildIRepoTemplate();

            context.AddSource($"{genRepoNames.IRepoName}.g.cs", template);
        }

        #endregion

        #region Helper Methods
        private static GeneratedUoWInfo GetGeneratedUoWInfo(List<GeneratedRepoNames> genRepoNames, UoWSourceGenerator settings)
        {
            var UoW_constractor = new StringBuilder();
            var UoW_Parameters = new StringBuilder();
            var UoW_Properties = new StringBuilder();
            var IUoW_Properties = new StringBuilder();

            var generatedInfo = new GeneratedUoWInfo();

            //Adding private property for the dbContext
            UoW_Properties.Append($"        private readonly {settings.DBContextName} _db; \n");
            UoW_Parameters.Append($"            {settings.DBContextName} db, \n");
            UoW_constractor.Append("            _db = db; \n");

            if (settings.EnableISP_Call)
            {
                UoW_Properties.Append("        public ISP_Call SP_Call { get; private set; } \n");
                UoW_Parameters.Append("            ISP_Call sP_Call, \n");
                UoW_constractor.Append("            SP_Call = sP_Call; \n");
                IUoW_Properties.Append("        ISP_Call SP_Call { get; } \n");
            }

            for (int i = 0; i < genRepoNames.Count; i++)
            {
                var isLast = i + 1 == genRepoNames.Count;

                UoW_constractor.Append($"            {genRepoNames[i].Entity} = {genRepoNames[i].RepoName}; {(!isLast ? "\n" : string.Empty)}");
                UoW_Parameters.Append($"            {genRepoNames[i].IRepoName} {genRepoNames[i].RepoName}{(!isLast ? ",\n" : string.Empty)}");
                UoW_Properties.Append($"        public {genRepoNames[i].IRepoName} {genRepoNames[i].Entity} " + $"{{get; private set;}} {(!isLast ? "\n" : string.Empty)}");
                IUoW_Properties.Append($"        {genRepoNames[i].IRepoName} {genRepoNames[i].Entity} " + $"{{get; }}{(!isLast ? "\n" : string.Empty)}");
            }

            generatedInfo.UoW_Constructor = UoW_constractor.ToString();
            generatedInfo.UoW_Parameters = UoW_Parameters.ToString();
            generatedInfo.UoW_Properties = UoW_Properties.ToString();
            generatedInfo.IUoW_Properties = IUoW_Properties.ToString();

            return generatedInfo;
        }

        #endregion
    }
}