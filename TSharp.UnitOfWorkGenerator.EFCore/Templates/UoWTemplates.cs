using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{
    internal static partial class BuildTemplate
    {
        public static string BuildUoWTemplate(this Template templateUoW)
        {
            var template = @"// Auto-generated code
{0} 

namespace {1} 
{{
    public partial class UnitOfWork : IUnitOfWork 
    {{ 
{2}

        public UnitOfWork 
        ( 
{3} 
        ) 
        {{ 
{4} 
        }}  

        public void Dispose()
        {{
            _db.Dispose();
        }}

        public void Save()
        {{
            _db.SaveChanges();
        }}

        public async Task SaveAsync()
        {{
            await _db.SaveChangesAsync();
        }}
    }} 
}}
";
            var uoWTemplate = string.Format(template, templateUoW.UsingStatements, templateUoW.Namespace, templateUoW.Properties, templateUoW.Parameters, templateUoW.Constructor);

            return uoWTemplate;
        }

        public static string BuildIUoWTemplate(this Template templateIUoW)
        {
            var template = @"// Auto-generated code
namespace {0} 
{{
    public partial interface IUnitOfWork : IDisposable 
    {{ 
{1} 
        void Save();
        Task SaveAsync();
    }} 
}}
";

            var iUoWTemplate = string.Format(template, templateIUoW.Namespace, templateIUoW.Properties);

            return iUoWTemplate;
        }


    }
}