using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TSharp.UnitOfWorkGenerator.EFCore.Tests.TestDefaults
{
    [TestClass]
    public class TestDefaults
    {
        [TestMethod]
        public void TestDefaultSettings()
        {
            // Create the 'input' compilation that the generator will act on
            Compilation inputCompilation = CreateCompilation(SourceInfo.GetSourceTrees());

            // directly create an instance of the generator
            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
            UnitOfWorkSourceGenerator generator = new UnitOfWorkSourceGenerator();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the generation pass
            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            // We can now assert things about the resulting compilation:
            var outputCompilationErrors = outputCompilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToList();

            Debug.Assert(diagnostics.IsEmpty);

            Debug.Assert(outputCompilationErrors.Count == 120); //these are known Compilation Errors. can't resolve these errors for the unit test but if there are more errors than 120 then something went wrong.

            // We can now assert things about the resulting compilation:
            Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
            Debug.Assert(outputCompilation.SyntaxTrees.Count() == 10); // we have 10 syntax trees, the original 'user' provided one, and 9 added by the generator

            // Or we can look at the results directly:
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            // The runResult contains the combined results of all generators passed to the driver
            Debug.Assert(runResult.GeneratedTrees.Length == 9);
            Debug.Assert(runResult.Diagnostics.IsEmpty);

            // Or you can access the individual results on a by-generator basis
            GeneratorRunResult generatorResult = runResult.Results[0];
            Debug.Assert(generatorResult.Generator == generator);
            Debug.Assert(generatorResult.Diagnostics.IsEmpty);
            Debug.Assert(generatorResult.GeneratedSources.Length == 9);
            Debug.Assert(generatorResult.Exception is null);

            var baseEntity = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "BaseEntity.g.cs").SourceText.ToString();
            var iBaseEntity = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "IBaseEntity.g.cs").SourceText.ToString();
            var repository = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "Repository.g.cs").SourceText.ToString();
            var iRepository = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "IRepository.g.cs").SourceText.ToString();
            var unitOfWork = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "UnitOfWork.g.cs").SourceText.ToString();
            var iUnitOfWork = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "IUnitOfWork.g.cs").SourceText.ToString();
            var employeeRepository = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "EmployeeRepository.g.cs").SourceText.ToString();
            var iEmployeeRepository = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "IEmployeeRepository.g.cs").SourceText.ToString();
            var employeeEntity = generatorResult.GeneratedSources.FirstOrDefault(x => x.HintName == "Entity_Employee.g.cs").SourceText.ToString();

            Debug.Assert(baseEntity.Equals(SourceInfo.ExpectedBaseEntity));
            Debug.Assert(iBaseEntity.Equals(SourceInfo.ExpectedIBaseEntity));
            Debug.Assert(repository.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedRepository.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(iRepository.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedIRepository.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(unitOfWork.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedUnitOfWOrk.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(iUnitOfWork.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedIUnitOfWork.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(employeeRepository.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedEmployeeRepository.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(iEmployeeRepository.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedIEmployeeRepository.Replace("\r\n", "\n").Replace("\r", "\n")));
            Debug.Assert(employeeEntity.Replace("\r\n", "\n").Replace("\r", "\n").Equals(SourceInfo.ExpectedEmployeeEntity.Replace("\r\n", "\n").Replace("\r", "\n")));

            static Compilation CreateCompilation(string source)
            {
                return CSharpCompilation.Create("TSharp.UnitOfWorkGenerator.API",
                    syntaxTrees: new[] { CSharpSyntaxTree.ParseText(source) },
                    references: Helpers.GetRequiredAssemblies(),

                    new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                );
            }
        }
    }
}
