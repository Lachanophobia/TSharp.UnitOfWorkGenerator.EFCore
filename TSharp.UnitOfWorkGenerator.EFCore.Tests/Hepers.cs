using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;
using TSharp.UnitOfWorkGenerator.API.Entities;
using TSharp.UnitOfWorkGenerator.EFCore.Utils;


namespace TSharp.UnitOfWorkGenerator.EFCore.Tests
{
    public class Helpers
    {
        public class MyAdditionalText : AdditionalText
        {
            private readonly string _text;

            public override string Path { get; }

            public MyAdditionalText(string path)
            {
                Path = path;
                _text = File.ReadAllText(path);
            }

            public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
            {
                return SourceText.From(_text);
            }
        }

        public static IEnumerable<MetadataReference> GetRequiredAssemblies()
        {
            string[] assemblies = Directory.GetFileSystemEntries(AppDomain.CurrentDomain.BaseDirectory + "DLLs", "*", SearchOption.AllDirectories);

            var references = new List<MetadataReference>();

            references.Add(MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location));

            //dbContextAssembly
            references.Add(MetadataReference.CreateFromFile(typeof(DbContext).GetTypeInfo().Assembly.Location));
            //utilsAssembly
            references.Add(MetadataReference.CreateFromFile(typeof(UoWGenerateRepository).GetTypeInfo().Assembly.Location));
            //apiAssembly
            references.Add(MetadataReference.CreateFromFile(typeof(Employee).GetTypeInfo().Assembly.Location));

            references.Add(MetadataReference.CreateFromFile(typeof(SqlServerDbContextOptionsExtensions).GetTypeInfo().Assembly.Location));

            foreach (var assembly in assemblies)
            {
                var metadataRef = MetadataReference.CreateFromFile(assembly);

                references.Add(metadataRef);
            }

            return references;
        }
    }
}
