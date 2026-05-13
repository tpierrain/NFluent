
namespace NFluent.RoslynAnalyzer.Tests
{
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using VerifyCS =
        NFluentCSharpCodeFixVerifier<Analyzer.NFluentAnalyzer, Analyzer.NFluentAnalyzerCodeFixProvider>;

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
        [Fact]
        public void ProcessEmptyText()
        {
            VerifyCS.VerifyAnalyzerAsync("").Wait();
        }

        [Theory]
        [InlineData("1", "IsNotZero")]
        [InlineData("\"hello\"", "IsNotEmpty")]
        [InlineData("true", "IsTrue")]
        [InlineData("new object()", "IsNotNull")]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }
    
        [Theory]
        [InlineData("DateTimeKind.Local")]
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

        [Theory]
        [InlineData("new [] {1,2}", "That(new [] {1,2})", "Not.IsEmpty")]
        [InlineData("new List<int>()", "That(new List<int>())", "Not.IsEmpty")]
        [InlineData("1", "That(1).As(\"test\")", "IsNotZero")]
        [InlineData("1", "That(1).Not.As(\"test\")", "IsNotZero")]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Theory]
        [InlineData("x == 1", "IsEqualTo")]
        [InlineData("x != 1", "IsNotEqualTo")]
        [InlineData("x > 1", "IsStrictlyGreaterThan")]
        [InlineData("x >= 1", "IsAfter")]
        [InlineData("x <= 1", "IsBefore")]
        [InlineData("x < 1", "IsStrictlyLessThan")]
        // should work if expression is reversed
        [InlineData("1 < x", "IsStrictlyGreaterThan")]
        [InlineData("1 <= x", "IsAfter")]
        [InlineData("1 >= x", "IsBefore")]
        [InlineData("1 > x", "IsStrictlyLessThan")]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);

        }

        [Fact]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Fact]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Fact]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Fact]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }

        [Fact]
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
            this.CheckAnalyzerAndFix(source, fixedSource, expected);
        }
    }
}
