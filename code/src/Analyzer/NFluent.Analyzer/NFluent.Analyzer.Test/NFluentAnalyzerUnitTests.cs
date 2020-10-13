using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = NFluent.Analyzer.Test.CSharpCodeFixVerifier<
    NFluent.Analyzer.NFluentAnalyzer,
    NFluent.Analyzer.NFluentAnalyzerCodeFixProvider>;

namespace NFluent.Analyzer.Test
{
    [TestClass]
    public class NFluentAnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            const string test = @"
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
                Check.That(10);
            }
        }
    }";

            var expected = VerifyCS.Diagnostic("NFluentAnalyzer");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixTest);
        }
    }
}
