using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{
    internal static partial class BuildTemplate
    {
        public static string BuildBaseEntityTemplate(this Template templateRepo)
        {
            var template = $@"// Auto-generated code
namespace {templateRepo.Namespace}
{{
    public partial class BaseEntity : IBaseEntity
    {{
    }}
}}
";

            return template;
        }

        public static string BuildIBaseEntityTemplate(this Template templateRepo)
        {
            var template = $@"// Auto-generated code
namespace {templateRepo.Namespace}
{{
    public partial interface IBaseEntity
    {{
    }}
}}
";

            return template;
        }

        public static string BuildDbEntity(this Template templateRepo)
        {
            var template = $@"// Auto-generated code
namespace {templateRepo.Namespace}
{{
    public partial class {templateRepo.Entity} : BaseEntity
    {{
    }}
}}
";
            return template;
        }
    }
}
