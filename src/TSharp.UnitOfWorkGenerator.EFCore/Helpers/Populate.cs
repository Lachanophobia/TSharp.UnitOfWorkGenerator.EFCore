using System.Text;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Helpers
{
    internal static class Populate
    {
        internal static void UnitOfWorkConstInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties, UoWSourceGenerator settings)
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

        internal static void UnitOfWorkGeneratedInfo(StringBuilder uoWConstructor, StringBuilder uoWParameters, StringBuilder uoWProperties, StringBuilder iUoWProperties, GeneratedRepoNames genRepoNames, bool isLast)
        {

            uoWConstructor.Append($"            {genRepoNames.Entity} = {genRepoNames.RepoName}; {(!isLast ? "\n" : string.Empty)}");
            uoWParameters.Append($"            {genRepoNames.IRepoName} {genRepoNames.RepoName}{(!isLast ? ",\n" : string.Empty)}");
            uoWProperties.Append($"        public {genRepoNames.IRepoName} {genRepoNames.Entity} " + $"{{get; private set;}} {(!isLast ? "\n" : string.Empty)}");
            iUoWProperties.Append($"        {genRepoNames.IRepoName} {genRepoNames.Entity} " + $"{{get; }}{(!isLast ? "\n" : string.Empty)}");
        }
    }
}
