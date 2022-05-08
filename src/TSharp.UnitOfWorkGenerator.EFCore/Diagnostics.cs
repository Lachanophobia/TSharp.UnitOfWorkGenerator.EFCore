using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSharp.UnitOfWorkGenerator.EFCore
{
    internal static class Diagnostics
    {
        internal static bool ValidateAppSettings(GeneratorExecutionContext context, AdditionalText file)
        {
            if (file?.GetText() == null)
            {
                var appSettingsFileMissing = new DiagnosticDescriptor(id: "UoW001",
                    title: "Could not get settings from uow.config.Json",
                    messageFormat: "Could not get settings from uow.config.Json, please review your json syntax. If you are happy with the default values remove the reference to the file from your .csproj.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(appSettingsFileMissing, Location.None));

                return false;
            }

            return true;
        }

        internal static bool ValidateReposToBeAdded(GeneratorExecutionContext context, List<TypeDeclarationSyntax> reposToBeAdded)
        {
            if (!reposToBeAdded.Any())
            {
                var error = new DiagnosticDescriptor(id: "UoW002",
                    title: "No dbEntities found",
                    messageFormat: "Could not find any dbEntities to generate the repositories.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Warning,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));
             
                return false;
            }

            return true;
        }

        internal static Tuple<TypeDeclarationSyntax, bool> ValidateDbContext(GeneratorExecutionContext context, List<TypeDeclarationSyntax> dbContext)
        {
            var dbContextCount = dbContext.Count;

            if (dbContextCount > 1)
            {
                var error = new DiagnosticDescriptor(id: "UoW003",
                    title: "The attribute [UoWDefineDbContext] is been used more than once",
                    messageFormat: "The attribute [UoWDefineDbContext] is been used more than once.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));

                return new Tuple<TypeDeclarationSyntax, bool>(null, false);
            }

            if (dbContextCount == 0)
            {
                var error = new DiagnosticDescriptor(id: "UoW004",
                    title: "The attribute [UoWDefineDbContext] is not been used",
                    messageFormat: "The attribute [UoWDefineDbContext] is not been used.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));

                return new Tuple<TypeDeclarationSyntax, bool>(null, false);
            }

            return new Tuple<TypeDeclarationSyntax, bool>(dbContext.FirstOrDefault(), true);
        }

        internal static bool CheckedForEntityFrameworkCoreDependency(GeneratorExecutionContext context)
        {
            if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase)))
            {
                var error = new DiagnosticDescriptor(id: "UoW005",
                    title: "Could not find assembly Microsoft.EntityFrameworkCore",
                    messageFormat: "Could not find assembly Microsoft.EntityFrameworkCore.",
                    category: "UoWGenerator",
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
                var error = new DiagnosticDescriptor(id: "UoW006",
                    title: "Could not find assembly Dapper",
                    messageFormat: "Could not find assembly Dapper.",
                    category: "UoWGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(error, Location.None));

                return false;
            }
            return true;
        }
    }
}
