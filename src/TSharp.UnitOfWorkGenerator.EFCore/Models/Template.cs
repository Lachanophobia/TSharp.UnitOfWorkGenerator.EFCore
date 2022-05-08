namespace TSharp.UnitOfWorkGenerator.EFCore.Models
{
    internal class Template
    {
        internal string UsingStatements { get; set; }
        internal string Namespace { get; set; }
        internal string IRepoName { get; set; }
        internal string RepoName { get; set; }
        internal string Entity { get; set; }
        internal string DBContextName { get; set; }
        internal string Properties { get; set; }
        internal string Parameters { get; set; }
        internal string Constructor { get; set; }
        internal string IdentityColumn { get; set; }
        internal string CustomRepository { get; set; }
    }
}
