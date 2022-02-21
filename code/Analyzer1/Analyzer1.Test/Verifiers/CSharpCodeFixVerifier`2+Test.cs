using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace NFluent.Analyzer.Test
{
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                    var nFluentAssembly = MetadataReference.CreateFromFile(typeof(Check).Assembly.Location);
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions)
                        .WithProjectMetadataReferences(projectId, solution.GetProject(projectId).MetadataReferences.Append(nFluentAssembly));
                    return solution;
                });
            }
        }
    }
}
