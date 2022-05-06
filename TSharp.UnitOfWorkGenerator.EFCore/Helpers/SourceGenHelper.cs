using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Helpers
{
    internal static class SourceGenHelper
    {
        internal static string GetNamespace(BaseTypeDeclarationSyntax syntax)
        {
            // If we don't have a namespace at all we'll return an empty string
            // This accounts for the "default namespace" case
            string nameSpace = string.Empty;

            // Get the containing syntax node for the type declaration
            // (could be a nested type, for example)
            SyntaxNode? potentialNamespaceParent = syntax.Parent;

            // Keep moving "out" of nested classes etc until we get to a namespace
            // or until we run out of parents
            while (potentialNamespaceParent != null &&
                   potentialNamespaceParent is not NamespaceDeclarationSyntax
                   && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            {
                potentialNamespaceParent = potentialNamespaceParent.Parent;
            }

            // Build up the final namespace by looping until we no longer have a namespace declaration
            if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
            {
                // We have a namespace. Use that as the type
                nameSpace = namespaceParent.Name.ToString();

                // Keep moving "out" of the namespace declarations until we 
                // run out of nested namespace declarations
                while (true)
                {
                    if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                    {
                        break;
                    }

                    // Add the outer namespace as a prefix to the final namespace
                    nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                    namespaceParent = parent;
                }
            }

            // return the final namespace
            return nameSpace;
        }

        internal static UoWSourceGenerator GetUoWSourceGeneratorSettings(GeneratorExecutionContext context, string dbContextName, string dbContextNamespace, string dbEntitiesNamespace)
        {
            var file = context.AdditionalFiles.FirstOrDefault(x => x.Path.Contains("uow.config.json"));
            var settings = new UoWSourceGenerator();

            if (file != null)
            {
                if (Diagnostics.ValidateAppSettings(context, file))
                {
                    settings = JsonConvert.DeserializeObject<AppSettings>(file.GetText().ToString()).UoWSourceGenerator;
                }
            }

            if (string.IsNullOrWhiteSpace(settings.IRepoNamespace))
            {
                settings.IRepoNamespace = $"{context.Compilation.AssemblyName}.Repositories.IRepository";
            }

            if (string.IsNullOrWhiteSpace(settings.RepoNamespace))
            {
                settings.RepoNamespace = $"{context.Compilation.AssemblyName}.Repositories.Repository";
            }

            settings.DBContextNamespace = dbContextNamespace;

            settings.DBContextName = dbContextName;

            settings.DBEntitiesNamespace = dbEntitiesNamespace;


            return settings;
        }

        internal static GeneratedUoWInfo GetGeneratedUoWInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties)
        {
            return new GeneratedUoWInfo()
            {
                UoW_Constructor = uoWConstructor.ToString(),
                UoW_Parameters = uoWParameters.ToString(),
                UoW_Properties = uoWProperties.ToString(),
                IUoW_Properties = iUoWProperties.ToString()
            };
        }

        internal static Tuple<TypeDeclarationSyntax, string, string> GetDbContextInfo(GeneratorExecutionContext context, List<TypeDeclarationSyntax> relevantDeclarationSyntaxes)
        {
            var dbContext = relevantDeclarationSyntaxes
                .Where(x => x.AttributeLists.Any(c => c.ToString().Equals("[UoWDefineDbContext]", StringComparison.OrdinalIgnoreCase))).ToList();

            var (dbContextDeclaration, isValid) = Diagnostics.ValidateDbContext(context, dbContext);

            if (!isValid)
            {
                return new Tuple<TypeDeclarationSyntax, string, string>(null, null, null);
            }

            var dbContextNamespace = SourceGenHelper.GetNamespace(dbContextDeclaration);

            return new Tuple<TypeDeclarationSyntax, string, string>(dbContextDeclaration, dbContextDeclaration.Identifier.ToString(), dbContextNamespace);
        }
    }
}
