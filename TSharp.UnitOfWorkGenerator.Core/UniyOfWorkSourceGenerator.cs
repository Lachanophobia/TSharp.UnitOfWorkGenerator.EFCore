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
            var genNamesList = new List<GeneratedRepoNames>();

            var reposToBeAdded = syntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[GenerateRepository]", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            //var reposUsingDirectives = reposToBeAdded.SelectMany(x => x.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Select(x => x.ToString()).Distinct();

            var settingsAsJson = context.AdditionalFiles.FirstOrDefault().GetText().ToString();
            var settings = JsonConvert.DeserializeObject<AppSettings>(settingsAsJson).UoWSourceGenerator;

            foreach (var repo in reposToBeAdded)
            {
                var entity = repo.Identifier.ToString();

                var genRepoNames = new GeneratedRepoNames()
                {
                    Entity = entity,
                    RepoName = $"{entity}Repository",
                    IRepoName = $"I{entity}Repository"
                };

                genNamesList.Add(genRepoNames);

                GenerateIRepo(genRepoNames, settings, context);
                GenerateRepo(genRepoNames, settings, context);
            }

            var generatedUoWInfo = new GeneratedUoWInfo();
            generatedUoWInfo = GetGeneratedUoWInfo(genNamesList);

            GenerateIUoW(genNamesList, generatedUoWInfo, settings, context);
            GenerateUoW(genNamesList, generatedUoWInfo, settings, context);
        }

        #region Generate Source Code
        private void GenerateUoW(List<GeneratedRepoNames> genRepoNames, GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.IRepoNamespace};";

            var template = UoWTemplates.GetUoWTemplate();
            var uoWTemplate = template.Replace("{UsingStatements}", defaultUsings)
                .Replace("{Namespace}", settings.RepoNamespace)
                .Replace("{Properties}", generatedInfo.UoW_Properties)
                .Replace("{ConstructorParameters}", generatedInfo.UoW_Parameters)
                .Replace("{Constructor}", generatedInfo.UoW_Constructor);

            context.AddSource($"UnitOfWork.g.cs", uoWTemplate);
        }

        private void GenerateIUoW(List<GeneratedRepoNames> genRepoNames, GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.IRepoNamespace};";

            var template = UoWTemplates.GetIUoWTemplate();
            var iUoWTemplate = template.Replace("{UsingStatements}", defaultUsings)
                .Replace("{Namespace}", settings.IRepoNamespace)
                .Replace("{Properties}", generatedInfo.IUoW_Properties);

            context.AddSource($"IUnitOfWork.g.cs", iUoWTemplate);
        }

        private void GenerateRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
               $"using {settings.DBEntitiesNamespace}; \n" +
               $"using {settings.IRepoNamespace};";

            var template = UoWTemplates.GetRepoTemplate();
            var repoTemplate = template.Replace("{UsingStatements}", defaultUsings)
                .Replace("{Namespace}", settings.RepoNamespace)
                .Replace("{RepoName}", genRepoNames.RepoName)
                .Replace("{Entity}", genRepoNames.Entity)
                .Replace("{Interface}", genRepoNames.IRepoName)
                .Replace("{DBContextName}", settings.DBContextName);

            context.AddSource($"{genRepoNames.RepoName}.g.cs", repoTemplate);
        }

        private void GenerateIRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
              $"using {settings.DBEntitiesNamespace};";

            var template = UoWTemplates.GetIRepoTemplate();
            var iRepoTemplate = template.Replace("{UsingStatements}", defaultUsings)
                .Replace("{Namespace}", settings.IRepoNamespace)
                .Replace("{IRepoName}", genRepoNames.IRepoName)
                .Replace("{Entity}", genRepoNames.Entity);

            context.AddSource($"{genRepoNames.IRepoName}.g.cs", iRepoTemplate);
        }

        #endregion

        #region Helper Methods
        private static GeneratedUoWInfo GetGeneratedUoWInfo(List<GeneratedRepoNames> genRepoNames)
        {
            var UoW_constractor = new StringBuilder();
            var UoW_Parameters = new StringBuilder();
            var UoW_Properties = new StringBuilder();
            var IUoW_Properties = new StringBuilder();

            var generatedInfo = new GeneratedUoWInfo();

            for (int i = 0; i < genRepoNames.Count; i++)
            {
                var isLast = i + 1 == genRepoNames.Count;

                UoW_constractor.Append($"           {genRepoNames[i].Entity} = {genRepoNames[i].RepoName}; {(!isLast ? "\n" : string.Empty)}");
                UoW_Parameters.Append($"           {genRepoNames[i].IRepoName} {genRepoNames[i].RepoName}{(!isLast ? ",\n" : string.Empty)}");
                UoW_Properties.Append($"       public {genRepoNames[i].IRepoName} {genRepoNames[i].Entity} " + $"{{get; private set;}} {(!isLast ? "\n" : string.Empty)}");
                IUoW_Properties.Append($"         {genRepoNames[i].IRepoName} {genRepoNames[i].Entity} " + $"{{get; }}{(!isLast ? "\n" : string.Empty)}");
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