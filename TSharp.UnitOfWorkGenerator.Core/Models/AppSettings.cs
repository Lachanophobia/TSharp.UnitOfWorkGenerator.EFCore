namespace TSharp.UnitOfWorkGenerator.Core.Models
{
    internal class AppSettings
    {
        public UoWSourceGenerator UoWSourceGenerator { get; set; }
    }

    internal class UoWSourceGenerator
    {
        public string IRepoNamespace { get; set; }
        public string RepoNamespace { get; set; }
        public string DBEntitiesNamespace { get; set; }
        public string DBContextName { get; set; }
    }
}