using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore
{
    internal static class Diagnostics
    {
        internal static bool ValidateAppSettings(GeneratorExecutionContext context, UoWSourceGenerator settings, AdditionalText file)
        {
            var allSettingsArePopulated = true;
            // latest code -> UoWGenerator007

            if (file == null)
            {
                var appSettingsFileMissing = new DiagnosticDescriptor(id: "UoWGenerator001",
                    title: "Could not get appsetting.json",
                    messageFormat: "Could not get appsettings.Json.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(appSettingsFileMissing, Location.None));

                allSettingsArePopulated = false;
            }

            if (string.IsNullOrWhiteSpace(settings.RepoNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator002",
                    title: "Could not getIRepositories Namespace",
                    messageFormat: "Could not get Repositories Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));

                allSettingsArePopulated = false;
            }

            if (string.IsNullOrWhiteSpace(settings.IRepoNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator003",
                    title: "Could not get IRepositories Namespace",
                    messageFormat: "Could not get IRepositories Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));

                allSettingsArePopulated = false;
            }

            if (string.IsNullOrWhiteSpace(settings.DBEntitiesNamespace))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator004",
                    title: "Could not get DBEntities Namespace",
                    messageFormat: "Could not get DBEntities Namespace, please check your appsettings.Json '{0}'.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));

                allSettingsArePopulated = false;
            }

            if (string.IsNullOrWhiteSpace(settings.DBContextName))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator005",
                    title: "Could not get DBContext Name",
                    messageFormat: "Could not get DBContext Name, please check your appsettings.Json '{0}'.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None, file.Path));

                allSettingsArePopulated = false;
            }

            return allSettingsArePopulated;
        }

        internal static bool ValidateReposToBeAdded(GeneratorExecutionContext context, List<TypeDeclarationSyntax> reposToBeAdded)
        {
            if (!reposToBeAdded.Any())
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator007",
                    title: "No dbEntities found",
                    messageFormat: "Could not find any dbEntities to generate the repositories.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Warning,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));
             
                return false;
            }

            return true;
        }

        internal static bool CheckedForEntityFrameworkCoreDependency(GeneratorExecutionContext context)
        {
            if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase)))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator005",
                    title: "Could not find assembly Microsoft.EntityFrameworkCore",
                    messageFormat: "Could not find assembly Microsoft.EntityFrameworkCore.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));
                
                return false;
            }
            
            return true;
        }

        internal static bool CheckedForDapperDependency(GeneratorExecutionContext context)
        {
            if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals("Dapper", StringComparison.OrdinalIgnoreCase)))
            {
                var error = new DiagnosticDescriptor(id: "UoWGenerator006",
                    title: "Could not find assembly Dapper",
                    messageFormat: "Could not find assembly Dapper.",
                    category: "UoWGeneratorGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));

                return false;
            }
            return true;
        }
    }
}
