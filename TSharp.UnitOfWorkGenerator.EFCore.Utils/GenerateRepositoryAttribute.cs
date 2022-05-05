using System;

namespace TSharp.UnitOfWorkGenerator.EFCore.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UoWGenerateRepository : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UoWDefineDbContext : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UoWOverrideRepository : Attribute
    {
    }
}