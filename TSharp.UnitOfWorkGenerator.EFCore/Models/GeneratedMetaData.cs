namespace TSharp.UnitOfWorkGenerator.EFCore.Models
{
    internal class GeneratedRepoNames
    {
        public GeneratedRepoNames(string entityName)
        {
            Entity = entityName;
            RepoName = $"{entityName}Repository";
            IRepoName = $"I{entityName}Repository";
        }

        public string Entity { get; set; }
        public string RepoName { get; set; }
        public string IRepoName { get; set; }
    }

    internal class GeneratedUoWInfo
    {
        public string IUoW_Properties { get; set; }
        public string UoW_Properties { get; set; }
        public string UoW_Parameters { get; set; }
        public string UoW_Constructor { get; set; }
    }
}