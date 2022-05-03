using System.Text;
using TSharp.UnitOfWorkGenerator.EFCore.Models;

namespace TSharp.UnitOfWorkGenerator.EFCore.Templates
{
    internal static partial class BuildTemplate
    {
        public static string BuildUoWTemplate(this Template templateUoW)
        {
            var template = $@"// Auto-generated code
{templateUoW.UsingStatements} 

namespace {templateUoW.Namespace} 
{{
    public partial class UnitOfWork : IUnitOfWork 
    {{ 
{templateUoW.Properties}

        public UnitOfWork 
        ( 
{templateUoW.Parameters} 
        ) 
        {{ 
{templateUoW.Constructor} 
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

            return template;
        }

        public static string BuildIUoWTemplate(this Template templateIUoW)
        {
            var template = $@"// Auto-generated code
namespace {templateIUoW.Namespace} 
{{
    public partial interface IUnitOfWork : IDisposable 
    {{ 
{templateIUoW.Properties} 
        void Save();
        Task SaveAsync();
    }} 
}}
";

            return template;
        }
    }
}