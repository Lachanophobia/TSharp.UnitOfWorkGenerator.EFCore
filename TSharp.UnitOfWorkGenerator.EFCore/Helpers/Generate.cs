using Microsoft.CodeAnalysis;
using TSharp.UnitOfWorkGenerator.EFCore.Models;
using TSharp.UnitOfWorkGenerator.EFCore.Templates;

namespace TSharp.UnitOfWorkGenerator.EFCore.Helpers
{
    internal static class Generate
    {
        internal static void dbEntity(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.DBEntitiesNamespace,
                Entity = genRepoNames.Entity
            }.BuildDbEntity();

            context.AddSource($"Entity_{genRepoNames.Entity}.g.cs", template);
        }

        internal static void BaseEntity(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.DBEntitiesNamespace
            }.BuildBaseEntityTemplate();

            context.AddSource("BaseEntity.g.cs", template);
        }

        internal static void IBaseEntity(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.DBEntitiesNamespace
            }.BuildIBaseEntityTemplate();

            context.AddSource("IBaseEntity.g.cs", template);
        }

        internal static void ISP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace
            }.BuildISP_CallTemplate();

            context.AddSource("ISP_Call.g.cs", template);
        }

        internal static void SP_Call(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.DBEntitiesNamespace}; \n" +
                $"using {settings.IRepoNamespace}; \n" +
                $"using {settings.DBContextNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                DBContextName = settings.DBContextName,
            }.BuildSP_CallTemplate();

            context.AddSource("SP_Call.g.cs", template);
        }

        internal static void BaseRepository(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.DBEntitiesNamespace}; \n" +
                $"using {settings.IRepoNamespace}; \n" +
                $"using {settings.DBContextNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.RepoNamespace,
                DBContextName = settings.DBContextName,
                IdentityColumn = settings.EnableGuidIdentityColumn ? "Guid" : "int"
            }.BuildBaseRepoTemplate();

            context.AddSource($"Repository.g.cs", template);
        }

        internal static void BaseIRepository(UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.DBEntitiesNamespace}; \n " +
                $"using {settings.DBContextNamespace};";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.IRepoNamespace,
                IdentityColumn = settings.EnableGuidIdentityColumn ? "Guid" : "int"
            }.BuildIBaseRepoTemplate();

            context.AddSource($"IRepository.g.cs", template);
        }

        internal static void UnitOfWork(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
                $"using {settings.IRepoNamespace}; \n" +
                $"using {settings.DBEntitiesNamespace}; \n" +
                $"using {settings.DBContextNamespace};";

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

        internal static void IUnitOfWork(GeneratedUoWInfo generatedInfo, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var template = new Template()
            {
                Namespace = settings.IRepoNamespace,
                Properties = generatedInfo.IUoW_Properties,
            }.BuildIUoWTemplate();

            context.AddSource($"IUnitOfWork.g.cs", template);
        }

        internal static void Repository(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
               $"using {settings.DBEntitiesNamespace}; \n" +
               $"using {settings.IRepoNamespace}; \n" +
               $"using {settings.DBContextNamespace};";

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

        internal static void IRepository(GeneratedRepoNames genRepoNames, UoWSourceGenerator settings, GeneratorExecutionContext context)
        {
            var defaultUsings =
              $"using {settings.DBEntitiesNamespace}; \n";

            var template = new Template()
            {
                UsingStatements = defaultUsings,
                Namespace = settings.IRepoNamespace,
                Entity = genRepoNames.Entity,
                IRepoName = genRepoNames.IRepoName,
            }.BuildIRepoTemplate();

            context.AddSource($"{genRepoNames.IRepoName}.g.cs", template);
        }
    }
}
