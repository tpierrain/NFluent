using VerifyCS = NFluent.Analyzer.Test.CSharpCodeFixVerifier<
    NFluent.Analyzer.NFluentAnalyzer,
    NFluent.Analyzer.NFluentAnalyzerCodeFixProvider>;

namespace NFluent.Analyzer.Test
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AnalyzerShould
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task ProcessEmptyText()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using NFluent;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public void Test()
            {
                {|#0:Check.That(0);|}
            }
        }
    }";

            var fixtest = @"
    using System;
    using NFluent;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public void Test()
            {
                Check.That(0).IsNotZero();
            }
        }
    }";
            var expected = VerifyCS.Diagnostic("NA0001").WithLocation(0).WithArguments(0);
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
