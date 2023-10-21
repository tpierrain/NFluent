// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DynamicTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

#if !DOTNET_20 && !DOTNET_30 && !NET35 && !DOTNET_40
namespace NFluent.Tests
{
    using System.Text;
    using Extensibility;
    using NUnit.Framework;

    using NFluent.Helpers;

    [TestFixture]
    public class DynamicTests
    {
        class Command
        {
            internal dynamic Subject { get; set; }
        }

        [Test]
        public void CanCheckNulls()
        {
            var cmd = new Command();
            dynamic sut = "test";

            Check.ThatDynamic(sut).IsNotNull();
            // this check fails
            Check.ThatCode(() => { Check.ThatDynamic(cmd.Subject).IsNotNull(); }).IsAFailingCheckWithMessage("", 
                "The checked dynamic is null whereas it must not.", 
                "The checked dynamic:", 
                "\t[null]");
        }

          // see GH #280
        [Test]
        public void SupportWithCustomMessage()
        {
            var cmd = new Command();
            dynamic sut = "test";

            AssertCheckFails(
                ()=>Check.WithCustomMessage("cool").ThatDynamic(cmd.Subject).IsNotNull(), 
                "cool",
                "The checked dynamic is null whereas it must not.",
                "The checked dynamic:",
                "\t[null]");
            // this check fails
            AssertCheckFails(
                ()=>Check.WithCustomMessage("cool").ThatDynamic(cmd.Subject).IsSameReferenceAs("tes"), 
                "cool",
                "The checked dynamic is not the expected reference.",
                "The checked dynamic:",
                "\t[null]",
                "The expected dynamic:",
                "\t[\"tes\"]");
            AssertCheckFails(
                ()=>Check.WithCustomMessage("cool").ThatDynamic(sut).IsEqualTo("tes"), 
                "cool",
                "The checked dynamic is not equal to the expected one.",
                "The checked dynamic:",
                "\t[\"test\"]",
                "The expected dynamic:",
                "\t[\"tes\"]");
            AssertCheckFails(
                ()=>Check.WithCustomMessage("cool").ThatDynamic(sut).Not.IsSameReferenceAs(sut), 
                "cool",
                "The checked dynamic is the expected reference whereas it must not.",
                "The checked dynamic:",
                "\t[\"test\"]",
                "The expected dynamic: different from",
                "\t[\"test\"]");
        }

        public static void AssertCheckFails(System.Action test, params Criteria[] message)
        {
            Check.ThatCode(test).IsAFailingCheckWithMessage(message);
        }

        [Test]
        public void AndWorks()
        {
            var cmd = new Command();
            dynamic sut = "test";

            Check.ThatDynamic(sut).IsNotNull().And.IsEqualTo("test");
            // this check fails
            Check.ThatCode(() =>
                Check.ThatDynamic(cmd.Subject).IsEqualTo(null).And.IsNotNull()).IsAFailingCheckWithMessage(
            "", 
            "The checked dynamic is null whereas it must not.", 
            "The checked dynamic:", 
            "\t[null]");
        }

        [Test]
        public void CanCheckReference()
        {
            dynamic sut = "test";

            Check.ThatDynamic(sut).IsSameReferenceAs(sut);
            Check.ThatCode(() => { Check.ThatDynamic(sut).IsSameReferenceAs("tes"); }).IsAFailingCheckWithMessage(
                "", 
                "The checked dynamic is not the expected reference.", 
                "The checked dynamic:", 
                "\t[\"test\"]", 
                "The expected dynamic:", 
                "\t[\"tes\"]");
        }

        [Test]
        public void CanCheckEquality()
        {
            dynamic sut = "test";

            Check.ThatDynamic(sut).IsEqualTo(sut);

            Check.ThatCode(() => { Check.ThatDynamic(sut).IsEqualTo("tes"); }).IsAFailingCheckWithMessage("",
                "The checked dynamic is not equal to the expected one.", 
                "The checked dynamic:", 
                "\t[\"test\"]", 
                "The expected dynamic:", 
                "\t[\"tes\"]");
        }

        [Test]
        public void NotWorks()
        {
            var cmd = new Command();
            dynamic sut = "test";

            Check.ThatDynamic(cmd.Subject).Not.IsNotNull();
            // this check fails
            AssertCheckFails(
                () => Check.ThatDynamic(sut).Not.IsNotNull(), 
                "",
                "The checked dynamic is not null whereas it must.",
                "The checked dynamic:",
                "\t[\"test\"]"
                );

            Check.ThatDynamic(sut).Not.IsEqualTo("tes");
            AssertCheckFails(
                () => Check.ThatDynamic(sut).Not.IsEqualTo(sut), 
                "",
                "The checked dynamic is equal to the expected one whereas it must not.",
                "The checked dynamic:",
                "\t[\"test\"]",
                "The expected dynamic: different from",
                "\t[\"test\"]"
                );

            Check.ThatDynamic(sut).Not.IsSameReferenceAs("tes");

            AssertCheckFails(
                () => Check.ThatDynamic(sut).Not.IsSameReferenceAs(sut), 
                "",
                "The checked dynamic is the expected reference whereas it must not.",
                "The checked dynamic:",
                "\t[\"test\"]",
                "The expected dynamic: different from",
                "\t[\"test\"]"
                );
        }

        [Test]
        public void ShouldChainEvenOnErrors()
        {
            // smoke test for chaining when check fails.
            var reporter = new MockReporter();
            dynamic sut = "test";
            using (var context = Check.ChangeReporterForScope(reporter))
            {
                Check.ThatDynamic(sut).Not.IsEqualTo(sut);
                // we should have an error messages.
                Check.That(reporter.Messages).IsNotEmpty();
                Check.ThatDynamic(sut).Not.IsSameReferenceAs(sut).And.Not.IsNotNull();
                Check.That(reporter.Messages.Length).IsStrictlyGreaterThan(150);
            }
        }


        private class MockReporter : IErrorReporter
        {
            private readonly StringBuilder storage = new StringBuilder();

            public string Messages => this.storage.ToString();

            public void ReportError(string message)
            {
                this.storage.Append(message);
            }

            public void Clear()
            {
                this.storage.Clear();
            }
        }
    }
}
#endif