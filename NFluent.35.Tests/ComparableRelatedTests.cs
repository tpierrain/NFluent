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

    using NUnit.Framework;

    [TestFixture]
    public class ComparableRelatedTests
    {
        #region IsBefore

        [Test]
        public void IsBeforeWorksWithVersion()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);
            Version v3 = new Version(3, 0);

            Check.That(v1).IsBefore(v2).And.IsBefore(v3);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[2.0]\nThe expected value: before\n\t[1.0]")]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);

            Check.That(v2).IsBefore(v1);
        }

        [Test]
        //[ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[null]\nThe expected value: before\n\t[1.0]")]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsInput()
        {
            Version v1 = new Version(1, 0);
            Version v2 = null;

            Check.That(v2).IsBefore(v1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[2.0]\nThe expected value: before\n\t[null]")]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsAfterValue()
        {
            Version v1 = null;
            Version v2 = new Version(2, 0);

            Check.That(v2).IsBefore(v1);
        }

        [Test]
        public void NotOperatorWorksWithIsBefore()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);

            Check.That(v2).Not.IsBefore(v1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is before the reference value whereas it must not.\nThe checked value:\n\t[1.0]\nThe expected value: after\n\t[2.0]")]
        public void IsBeforeWithNotOperatorCanThrowException()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);

            Check.That(v1).Not.IsBefore(v2);
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not after the reference value.\nThe checked value:\n\t[1.0]\nThe expected value: after\n\t[2.0]")]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.That(v1).IsAfter(v2);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is after the reference value whereas it must not.\nThe checked value:\n\t[2.0]\nThe expected value: before\n\t[1.0]")]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);

            Check.That(v2).Not.IsAfter(v1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is null so not after the reference value.\nThe checked value:\n\t[null]\nThe expected value: after\n\t[1.0]")]
        public void IsAfterDoesNotThrowNullReferenceExceptionWithNullAsInput()
        {
            var v1 = new Version(1, 0);
            Version v2 = null;

            Check.That(v2).IsAfter(v1);
        }

        #endregion
    }
}
