using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = NFluent.Analyzer.Test.CSharpCodeFixVerifier<
    NFluent.Analyzer.NFluentAnalyzer,
    NFluent.Analyzer.NFluentAnalyzerCodeFixProvider>;
using VerifyCSAnalyzer = NFluent.Analyzer.Test.CSharpAnalyzerVerifier<NFluent.Analyzer.NFluentAnalyzer>;

namespace NFluent.Analyzer.Test
{
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
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
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments("10").WithLocation(16,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }

        [TestMethod]
        public async Task DontReportCheckThatIsNotNullExpression()
        {
            const string testCode = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
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
        [TestMethod]
        public async Task ReportAndFixCheckThatExpression()
        {
            const string testCode = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.That(this);
            }
        }
    }";

            const string fixTest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using NFluent;

    namespace ConsoleApplication1
    {
        class TestClass
        {
            public void ShouldDetectIncompleteExpression()
            {
                Check.That(this).IsNotNull();
            }
        }
    }";

            var referenceAssemblies = ReferenceAssemblies.Default.AddPackages(ImmutableArray.Create(new PackageIdentity("NFluent", "2.7.0")));
            var test = new VerifyCS.Test
            {
                TestCode =  testCode,
                FixedCode = fixTest,
                ExpectedDiagnostics = {VerifyCS.Diagnostic("NFluentAnalyzer").WithArguments("this").WithLocation(16,17)},
                ReferenceAssemblies = referenceAssemblies
            };

            await test.RunAsync();
        }
    }
}
