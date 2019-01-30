// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ComparableRelatedTests.cs" company="">
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
namespace NFluent.Tests
{
    using System;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ComparableRelatedTests
    {
        #region IsBefore

        [Test]
        public void IsBeforeWorksWithVersion()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);
            var v3 = new Version(3, 0);

            Check.That(v1).IsBefore(v2).And.IsBefore(v3);
        }

        [Test]
        public void IsBeforeFailsWhenEqual()
        {
            var v1 = new Version(1, 0);
            Check.ThatCode(() => { Check.That(v1).IsBefore(v1); }).IsAFailingCheckWithMessage(
                "",
                "The checked value is not before the reference value.",
                "The checked value:",
                "\t[1.0]",
                "The expected value: before",
                "\t[1.0]");
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.ThatCode(() => Check.That(v2).IsBefore(v1))
                    .IsAFailingCheckWithMessage("",
                    "The checked value is not before the reference value.",
                    "The checked value:",
                    "\t[2.0]",
                    "The expected value: before",
                    "\t[1.0]");
        }

        [Test]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsInput()
        {
            var v1 = new Version(1, 0);
            Check.That((Version) null).IsBefore(v1);
        }

        [Test]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsAfterValue()
        {
            var v2 = new Version(2, 0);

            Check.ThatCode(() => Check.That(v2).IsBefore(null))
                    .IsAFailingCheckWithMessage("",
                    "The checked value is not before the reference value.",
                    "The checked value:", "\t[2.0]",  "The expected value: before", "\t[null]");
        }

        [Test]
        public void NotOperatorWorksWithIsBefore()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.That(v2).Not.IsBefore(v1);
        }

        [Test]
        public void IsBeforeWithNotOperatorCanThrowException()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.ThatCode(() => Check.That(v1).Not.IsBefore(v2))
                    .IsAFailingCheckWithMessage("",
                    "The checked value is before the reference value whereas it must not.",
                    "The checked value:","\t[1.0]", "The expected value: after", "\t[2.0]");
        }

        #endregion

        #region IsAfter

        [Test]
        public void IsAfterWorks()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.That(v2).IsAfter(v1);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.That(v1).Not.IsAfter(v2);
        }

        [Test]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.ThatCode(() => Check.That(v1).IsAfter(v2))
                    .IsAFailingCheckWithMessage("", "The checked value is not after the reference value.",
                    "The checked value:", "\t[1.0]", "The expected value: after", "\t[2.0]");
        }

        [Test]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.ThatCode(() => Check.That(v2).Not.IsAfter(v1))
                    .IsAFailingCheckWithMessage("", "The checked value is after the reference value whereas it must not.", 
                    "The checked value:","\t[2.0]", "The expected value: before", "\t[1.0]");
        }

        [Test]
        public void IsAfterDoesNotThrowNullReferenceExceptionWithNullAsInput()
        {
            var v1 = new Version(1, 0);
            Check.ThatCode(() => Check.That((Version) null).IsAfter(v1))
                    .IsAFailingCheckWithMessage("", "The checked value is null so not after the reference value.", "The expected value: after", "\t[1.0]");
        }

        [Test]
        public void IsAfterFailsWhenEqual()
        {
            var v1 = new Version(1, 0);
            Check.ThatCode(() => { Check.That(v1).IsAfter(v1); }).IsAFailingCheckWithMessage(
                "",
                "The checked value is not after the reference value.",
                "The checked value:",
                "\t[1.0]",
                "The expected value: after",
                "\t[1.0]");
        }

        #endregion
    }
}
