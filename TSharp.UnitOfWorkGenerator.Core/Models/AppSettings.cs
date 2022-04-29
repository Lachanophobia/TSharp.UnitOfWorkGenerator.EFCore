namespace TSharp.UnitOfWorkGenerator.Core
{
    internal class AppSettings
    {
        public UoWSourceGenerator UoWSourceGenerator { get; set; }
    }

    internal class UoWSourceGenerator
    {
        public string IGenRepoNamespace { get; set; }
        public string GenRepoNamespace { get; set; }
        public string DBEntitiesNamespace { get; set; }
        public string DBContextName { get; set; }
    }
}