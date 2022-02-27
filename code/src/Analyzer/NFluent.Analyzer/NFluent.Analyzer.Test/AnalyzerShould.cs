using VerifyCS = NFluent.Analyzer.Test.CSharpCodeFixVerifier<
    NFluent.Analyzer.NFluentAnalyzer,
    NFluent.Analyzer.NFluentAnalyzerCodeFixProvider>;

namespace NFluent.Analyzer.Test
{
    using Microsoft.CodeAnalysis.Testing;
    using NUnit.Framework;

    [TestFixture]
    public class AnalyzerShould
    {
        // inject test (incorrect) code, verify the analyzer find the expected anomaly and generates a fix equivalent to fixed code
        private void CheckAnalyzerAndFix(string originalCode, string fixedCode, DiagnosticResult diagnostic)
        {

            var template = @"using System;
    using NFluent;
    using System.Collections.Generic;
    namespace ConsoleApplication1
    {{
        class TypeName
        {{   {0}
        }}
    }}";
            var source = string.Format(template, originalCode);
            var fixedSource = string.Format(template, fixedCode);
            VerifyCS.VerifyCodeFixAsync(source, diagnostic, fixedSource).Wait();
        }

        //No diagnostics expected to show up
        [Test]
        public void ProcessEmptyText()
        {
            VerifyCS.VerifyAnalyzerAsync("").Wait();
        }

        [TestCase("1", "IsNotZero")]
        [TestCase("\"hello\"", "IsNotEmpty")]
        [TestCase("true", "IsTrue")]
        [TestCase("new object()", "IsNotNull")]
        public void ReportStandaloneCheckThatAndProvideFixForSimpleCase(string sut, string check)
        {
            var source = @"public void Test()
            {
                {|#0:Check.That("+sut+@");|}
            }";

            var fixedSource = @"public void Test()
            {
                Check.That("+sut+")."+check+@"();
            }";
            var expected = VerifyCS.Diagnostic("NA0001").WithLocation(0).WithArguments(sut);
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }
    
        [TestCase("DateTimeKind.Local")]
        public void ReportStandaloneCheckThat(string sut)
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
                {|#0:Check.That("+sut+@");|}
            }
        }
    }";

            var expected = VerifyCS.Diagnostic("NA0001").WithLocation(0).WithArguments(sut);
            VerifyCS.VerifyAnalyzerAsync(test, expected).Wait();
        }

        [TestCase("new [] {1,2}", "That(new [] {1,2})", "Not.IsEmpty")]
        [TestCase("new List<int>()", "That(new List<int>())", "Not.IsEmpty")]
        [TestCase("1", "That(1).As(\"test\")", "IsNotZero")]
        [TestCase("1", "That(1).Not.As(\"test\")", "IsNotZero")]
        public void ReportStandaloneCheckThatAndProvideFix(string sut, string check, string fix)
        {
            var source = @"public void Test()
            {
                {|#0:Check."+check+@";|}
            }";

            var fixedSource = @"public void Test()
            {
                Check."+check+"."+fix+@"();
            }";
            var expected = VerifyCS.Diagnostic("NA0001").WithLocation(0).WithArguments(sut);
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [TestCase("x == 1", "IsEqualTo")]
        [TestCase("x != 1", "IsNotEqualTo")]
        [TestCase("x > 1", "IsStrictlyGreaterThan")]
        [TestCase("x >= 1", "IsAfter")]
        [TestCase("x <= 1", "IsBefore")]
        [TestCase("x < 1", "IsStrictlyLessThan")]
        // should work if expression is reversed
        [TestCase("1 < x", "IsStrictlyGreaterThan")]
        [TestCase("1 <= x", "IsAfter")]
        [TestCase("1 >= x", "IsBefore")]
        [TestCase("1 > x", "IsStrictlyLessThan")]
        public void ReportBadBinaryExpressionCheckAndFIx(string expression, string check)
        {
            var source = @"private int x;
            public void Test()
            {
                {|#0:Check.That("+expression+@").IsTrue();|}
            }";

            var fixedSource = @"private int x;
            public void Test()
            {
                Check.That(x)."+check+@"(1);
            }";
            var expected = VerifyCS.Diagnostic("NA0002").WithLocation(0).WithArguments("x", check);
            CheckAnalyzerAndFix(source, fixedSource, expected);

        }

        [Test]
        public void ReplaceCheckOnCountForVariable()
        {

            var source = @"
            public void Test()
            {
                var x = new List<int>();
                {|#0:Check.That(x.Count).IsEqualTo(10);|}
            }";

            var fixedSource = @"
            public void Test()
            {
                var x = new List<int>();
                Check.That(x).CountIs(10);
            }";
            var expected = VerifyCS.Diagnostic("NA0003").WithLocation(0).WithArguments("x", "CountIs"); 
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Test]
        public void ReplaceCheckOnCountForField()
        {

            var source = @"
            List<int> x = new List<int>();
            public void Test()
            {
                {|#0:Check.That(x.Count).IsEqualTo(10);|}
            }";

            var fixedSource = @"
            List<int> x = new List<int>();
            public void Test()
            {
                Check.That(x).CountIs(10);
            }";
            var expected = VerifyCS.Diagnostic("NA0003").WithLocation(0).WithArguments("x", "CountIs"); 
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Test]
        public void ReplaceCheckOnCountForProperty()
        {

            var source = @"
            List<int> x {get;set;}
            public void Test()
            {
                {|#0:Check.That(x.Count).IsEqualTo(10);|}
            }";

            var fixedSource = @"
            List<int> x {get;set;}
            public void Test()
            {
                Check.That(x).CountIs(10);
            }";
            var expected = VerifyCS.Diagnostic("NA0003").WithLocation(0).WithArguments("x", "CountIs"); 
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Test]
        public void ReplaceCheckOnCountFromParameter()
        {

            var source = @"
            public void Test(List<int> x)
            {
                {|#0:Check.That(x.Count).IsEqualTo(10);|}
            }";

            var fixedSource = @"
            public void Test(List<int> x)
            {
                Check.That(x).CountIs(10);
            }";
            var expected = VerifyCS.Diagnostic("NA0003").WithLocation(0).WithArguments("x", "CountIs"); 
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Test]
        public void ReplaceCheckOnIsEqualToDefault()
        {

            var source = @"
            public void Test()
            {
                {|#0:Check.That(0).IsEqualTo(default);|}
            }";

            var fixedSource = @"
            public void Test()
            {
                Check.That(0).IsDefaultValue();
            }";
            var expected = VerifyCS.Diagnostic("NA0101").WithLocation(0); 
            CheckAnalyzerAndFix(source, fixedSource, expected);
        }
    }
}
