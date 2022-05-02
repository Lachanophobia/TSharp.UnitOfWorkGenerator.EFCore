using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TSharp.UnitOfWorkGenerator.Core.Models;
using TSharp.UnitOfWorkGenerator.Core.Templates;

namespace TSharp.UnitOfWorkGenerator.Core
{
    [Generator]
    public partial class UnityOfWorkSourceGenerator : ISourceGenerator
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

            var reposToBeAdded = syntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is TypeDeclarationSyntax)
                .Cast<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[GenerateRepository]", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            //var reposUsingDirectives = reposToBeAdded.SelectMany(x => x.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Select(x => x.ToString()).Distinct();


            var settings = GetAppSettings(context);

            GenerateBaseIRepo(settings, context);
            GenerateBaseRepo(settings, context);

            if (settings.EnableISP_Call)
            {
                GenerateISP_Call(settings, context);
                GenerateSP_Call(settings, context);
            }

            StringBuilder uoWConstructor = new(), uoWParameters = new(), uoWProperties = new(), iUoWProperties = new();

            PopulateUoWConstInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, settings);

            for (int i = 0; i < reposToBeAdded.Count; i++)
            {
                var entity = reposToBeAdded[i].Identifier.ToString();
                var isLast = i + 1 == reposToBeAdded.Count;

                var genRepoNames = new GeneratedRepoNames()
                {
                    Entity = entity,
                    RepoName = $"{entity}Repository",
                    IRepoName = $"I{entity}Repository"
                };

                GenerateIRepo(genRepoNames, settings, context);
                GenerateRepo(genRepoNames, settings, context);

                PopulateUoWGeneratedInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, genRepoNames, isLast);
            }

            var generatedUoWInfo = GetGeneratedUoWInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties);

            GenerateIUoW(generatedUoWInfo, settings, context);
            GenerateUoW(generatedUoWInfo, settings, context);
        }



        #region Generate Source Code

        private static void GenerateISP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace
            }.BuildISP_CallTemplate();

            context.AddSource("ISP_Call.g.cs", template);
        }

        private static void GenerateSP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
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

        private static void GenerateBaseRepo(UoWSourceGenerator settings, GeneratorExecutionContext context)
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

        private static void GenerateBaseIRepo(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace,
                IdentityColumn = settings.EnableGuidIdentityColumn ? "Guid" : "int"
            }.BuildIBaseRepoTemplate();

            context.AddSource($"IRepository.g.cs", template);
        }

        private static void GenerateUoW(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
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

        private static void GenerateIUoW(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace,
                Properties = generatedInfo.IUoW_Properties,
            }.BuildIUoWTemplate();

            context.AddSource($"IUnitOfWork.g.cs", template);
        }

        private static void GenerateRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
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

        private static void GenerateIRepo(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
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
        private static void PopulateUoWConstInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties, UoWSourceGenerator settings)
        {
            uoWProperties.Append($"        private readonly {settings.DBContextName} _db; \n");
            uoWParameters.Append($"            {settings.DBContextName} db, \n");
            uoWConstructor.Append("            _db = db; \n");

            if (settings.EnableISP_Call)
            {
                uoWProperties.Append("        public ISP_Call SP_Call { get; private set; } \n");
                uoWParameters.Append("            ISP_Call sP_Call, \n");
                uoWConstructor.Append("            SP_Call = sP_Call; \n");
                iUoWProperties.Append("        ISP_Call SP_Call { get; } \n");
            }
        }

        private static void PopulateUoWGeneratedInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties, GeneratedRepoNames genRepoNames, bool isLast)
        {

            uoWConstructor.Append($"            {genRepoNames.Entity} = {genRepoNames.RepoName}; {(!isLast ? "\n" : string.Empty)}");
            uoWParameters.Append($"            {genRepoNames.IRepoName} {genRepoNames.RepoName}{(!isLast ? ",\n" : string.Empty)}");
            uoWProperties.Append($"        public {genRepoNames.IRepoName} {genRepoNames.Entity} " + $"{{get; private set;}} {(!isLast ? "\n" : string.Empty)}");
            iUoWProperties.Append($"        {genRepoNames.IRepoName} {genRepoNames.Entity} " + $"{{get; }}{(!isLast ? "\n" : string.Empty)}");
        }

        private GeneratedUoWInfo GetGeneratedUoWInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties)
        {
            return new GeneratedUoWInfo()
            {
                UoW_Constructor = uoWConstructor.ToString(),
                UoW_Parameters = uoWParameters.ToString(),
                UoW_Properties = uoWProperties.ToString(),
                IUoW_Properties = iUoWProperties.ToString()
            };
        }

        private UoWSourceGenerator GetAppSettings(GeneratorExecutionContext context)
        {
            var file = context.AdditionalFiles.FirstOrDefault(x => x.Path.Contains("appsettings.json"));

            var appSettingsFileMissing = new DiagnosticDescriptor(id: "UoW001",
                title: "Could not get appsetting.json",
                messageFormat: "Could not get appsettings.Json '{0}'.",
                category: "UoWGenerator",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true);

            if (file == null)
                context.ReportDiagnostic(Diagnostic.Create(appSettingsFileMissing, Location.None));

            var settingsAsJson = file.GetText().ToString();
            var setting = JsonConvert.DeserializeObject<AppSettings>(settingsAsJson).UoWSourceGenerator;

            if (string.IsNullOrWhiteSpace(setting.RepoNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoW002",
                    title: "Could not getIRepositories Namespace",
                    messageFormat: "Could not get Repositories Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));
            }

            if (string.IsNullOrWhiteSpace(setting.IRepoNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoW003",
                    title: "Could not get IRepositories Namespace",
                    messageFormat: "Could not get IRepositories Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));
            }

            if (string.IsNullOrWhiteSpace(setting.DBEntitiesNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoW004",
                    title: "Could not get DBEntities Namespace",
                    messageFormat: "Could not get DBEntities Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));
            }

            if (string.IsNullOrWhiteSpace(setting.DBContextName))
            {
                var error = new DiagnosticDescriptor(id: "UoW005",
                    title: "Could not get DBContext Name",
                    messageFormat: "Could not get DBContext Name, please check your appsettings.Json '{0}'.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));
            }

            return setting;
        }
        #endregion
    }
}