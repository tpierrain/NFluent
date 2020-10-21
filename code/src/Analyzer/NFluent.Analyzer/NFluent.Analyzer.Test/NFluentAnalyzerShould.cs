using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = NFluent.Analyzer.Test.CSharpCodeFixVerifier<
    NFluent.Analyzer.NFluentAnalyzer,
    NFluent.Analyzer.NFluentAnalyzerCodeFixProvider>;
using VerifyCSAnalyzer = NFluent.Analyzer.Test.CSharpAnalyzerVerifier<NFluent.Analyzer.NFluentAnalyzer>;

namespace NFluent.Analyzer.Test
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis.Testing;

    [TestClass]
    public class NFluentAnalyzerShould
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task StaySilentWhenNoCode()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // Diagnostic test
        [TestMethod]
        public async Task ReportCheckThatExpression()
        {
            const string testCode = @"
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.That(10);
            }
        }
    }";

            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCSAnalyzer.Test
            {
                TestCode =  testCode,
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments("10").WithLocation(10,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

        [TestMethod]
        public async Task DontReportProperCheckThatExpression()
        {
            const string testCode = @"
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.That(10).IsZero();
            }
        }
    }";

            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCSAnalyzer.Test
            {
                TestCode =  testCode,
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

        // Diagnostic test
        [DataTestMethod]
        [DataRow("this", "IsNotNull()")]
        [DataRow("true", "IsTrue()")]
        [DataRow("10", "IsNotZero()")]
        [DataRow("10.0", "IsNotZero()")]
        [DataRow("10m", "IsNotZero()")]
        [DataRow("10.0f", "IsNotZero()")]
        [DataRow("new []{1,2}", "Not.IsEmpty()")]
        [DataRow("new List<int>{1}", "Not.IsEmpty()")]
        [DataRow("(int?)2", "IsNotNull()")]
        [DataRow("\"Test\"", "IsNotEmpty()")]
        public async Task ReportAndFixCheckThatExpression(string sut, string check)
        {
            var testCode = $@"
    using NFluent;
    using System.Collections.Generic;

    namespace ConsoleApplication1
    {{
        class TestClass
        {{
            public void ShouldDetectIncompleteExpression()
            {{
                Check.That({sut});
            }}
        }}
    }}";

            var fixTest = $@"
    using NFluent;
    using System.Collections.Generic;

    namespace ConsoleApplication1
    {{
        class TestClass
        {{
            public void ShouldDetectIncompleteExpression()
            {{
                Check.That({sut}).{check};
            }}
        }}
    }}";
            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCS.Test
            {
                TestCode =  testCode,
                FixedCode = fixTest,
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments(sut).WithLocation(11,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

        [TestMethod]
        public async Task ReportCheckWithCustomMessage()
        {
            const string testCode = @"
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.WithCustomMessage(""dont care"").That(10);
            }
        }
    }";

            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCSAnalyzer.Test
            {
                TestCode =  testCode,
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments(10).WithLocation(10,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

        [TestMethod]
        public async Task ReportCheckWithAs()
        {
            const string testCode = @"
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.That(10).As(""number"");
            }
        }
    }";

            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCSAnalyzer.Test
            {
                TestCode =  testCode,
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments(10).WithLocation(10,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

    }
}