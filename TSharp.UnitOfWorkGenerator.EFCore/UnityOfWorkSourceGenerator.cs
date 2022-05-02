using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using TSharp.UnitOfWorkGenerator.EFCore.Templates;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore
{
    [Generator]
    public partial class UnityOfWorkSourceGenerator : IIncrementalGenerator
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
            //Diagnostics.CheckedForEntityFrameworkCoreDependency(context);

            //var syntaxTrees = context.Compilation.SyntaxTrees;

            //var reposToBeAdded = syntaxTrees
            //    .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
            //    .Where(x => x is TypeDeclarationSyntax)
            //    .Cast<TypeDeclarationSyntax>()
            //    .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[GenerateRepository]", StringComparison.OrdinalIgnoreCase)))
            //    .ToList();

            ////var reposUsingDirectives = reposToBeAdded.SelectMany(x => x.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).Select(x => x.ToString()).Distinct();

            //var (settings, file) = GetAppSettings(context);

            //Diagnostics.ValidateAppSettings(context, settings, file);
            //Diagnostics.CheckedForDapperDependency(context, settings);

            //GenerateBaseIRepo(settings, context);
            //GenerateBaseRepo(settings, context);

            //if (settings.EnableISP_Call)
            //{
            //    GenerateISP_Call(settings, context);
            //    GenerateSP_Call(settings, context);
            //}

            //StringBuilder uoWConstructor = new(), uoWParameters = new(), uoWProperties = new(), iUoWProperties = new();

            //PopulateUoWConstInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, settings);

            //for (int i = 0; i < reposToBeAdded.Count; i++)
            //{
            //    var entity = reposToBeAdded[i].Identifier.ToString();
            //    var isLast = i + 1 == reposToBeAdded.Count;

            //    var genRepoNames = new GeneratedRepoNames()
            //    {
            //        Entity = entity,
            //        RepoName = $"{entity}Repository",
            //        IRepoName = $"I{entity}Repository"
            //    };

            //    GenerateIRepo(genRepoNames, settings, context);
            //    GenerateRepo(genRepoNames, settings, context);

            //    PopulateUoWGeneratedInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties, genRepoNames, isLast);
            //}

            //var generatedUoWInfo = GetGeneratedUoWInfo(uoWConstructor, uoWParameters, uoWProperties, iUoWProperties);

            //GenerateIUoW(generatedUoWInfo, settings, context);
            //GenerateUoW(generatedUoWInfo, settings, context);
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

        private Tuple<UoWSourceGenerator, AdditionalText> GetAppSettings(IncrementalGeneratorInitializationContext context)
        {
            //var file = context.AdditionalFiles.FirstOrDefault(x => x.Path.Contains("appsettings.json"));

            //Diagnostics.CheckForAppSettingsExistence(context, file);

            //var settingsAsJson = file.GetText().ToString();
            //var settings = JsonConvert.DeserializeObject<AppSettings>(settingsAsJson).UoWSourceGenerator;

            //IncrementalValuesProvider<AdditionalText> textFiles = context.AdditionalTextsProvider.Where(static file
            //    => file.Path.Contains("appsettings.json"));



            IncrementalValuesProvider<AdditionalText> textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.Contains("appsettings.json"));
            IncrementalValuesProvider<(string name, string content)> namesAndContents = textFiles.Select((text, cancellationToken) => (name: Path.GetFileNameWithoutExtension(text.Path), content: text.GetText(cancellationToken)!.ToString()));

            string test2 = "";
            string test3 = "";

            // generate a class that contains their values as const strings
            context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
            {

                test2 += nameAndContent.content;
                test3 += nameAndContent.name;
            });

            var test = 0;

            throw new NotImplementedException();

            //var settings = JsonConvert.DeserializeObject<AppSettings>(nameAndContent.Select(x => x.path)).UoWSourceGenerator;

            //return new Tuple<UoWSourceGenerator, AdditionalText>(settings, file);
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
            Debug.WriteLine("Initalize code generator");

            var settings = GetAppSettings(context);

            IncrementalValuesProvider<TypeDeclarationSyntax> classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            IncrementalValueProvider<(Compilation, ImmutableArray<TypeDeclarationSyntax>)> compilationAndClasses
                = context.CompilationProvider.Combine(classDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndClasses, static (spc, source) =>
            {
                Execute2(source.Item1, source.Item2, spc);
            });


            static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            {
                return node is TypeDeclarationSyntax m && m.AttributeLists.Any();
            }


            static TypeDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
            {
                // we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
                var typeDeclarationSyntax = ((TypeDeclarationSyntax)context.Node);


                //Diagnostics.CheckedForEntityFrameworkCoreDependency(context);

                foreach (AttributeListSyntax attributeListSyntax in typeDeclarationSyntax.AttributeLists)
                {
                    foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
                    {

                        IMethodSymbol attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;
                        if (attributeSymbol == null)
                        {
                            // weird, we couldn't get the symbol, ignore it
                            continue;
                        }

                        INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                        string fullName = attributeContainingTypeSymbol.ToDisplayString();

                        // Is the attribute the [LoggerMessage] attribute?
                        if (fullName == "TSharp.UnitOfWorkGenerator.EFCore.Utils.GenerateRepository")
                        {
                            // return the parent class of the method
                            return typeDeclarationSyntax as TypeDeclarationSyntax;
                        }
                    }
                }

               

                return null;

            }

        }

        private static void Execute2(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            Diagnostics.CheckedForEntityFrameworkCoreDependency(compilation, context);

            IEnumerable<TypeDeclarationSyntax> distinctClasses = classes.Distinct();

            throw new NotImplementedException();
        }


        #endregion
    }
}
