// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectRelatedTest.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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
    using NFluent.Tests.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectRelatedTest
    {
        [Test]
        public void IsSameReferenceAsWorks()
        {
            var test = new object();
            Check.That(test).IsSameReferenceAs(test);
        }

        [Test]
        public void IsSameReferenceAsDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new[] { 1, 4, 42 };
            Check.That(numbers).IsSameReferenceAs(numbers).And.Contains(42);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must be the same instance than expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: same instance than\n\t[System.Object]")]
        public void IsSameReferenceFailsProperly()
        {
            Check.That(new object()).IsSameReferenceAs(new object());
        }

        [Test]
        public void IsDistinctWorks()
        {
            Check.That(new object()).IsDistinctFrom(new object());
        }
        
        [Test]
        public void IsDistinctFromDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new[] { 1, 4, 42 };
            var otherNumbers = new[] { 7, 8, 9 };
            Check.That(numbers).IsDistinctFrom(otherNumbers).And.Contains(42);
        }

        [Test]
        public void IsNullWorks()
        {
            Check.That((object)null).IsNull();
        }

        [Test]
        public void IsNullWorksWithNullable()
        {
            Check.That((int?)null).IsNull();
        }

        [Test]
        public void IsNullDoesNotLoseOriginalTypeForOtherCheck()
        {
            var values = new[] { 0, 1, 2 };
            Check.That(values).Not.IsNull().And.HasSize(3);
        }

        [Test]
        public void IsNotNullWorksWithNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value is null whereas it must not.")]
        public void IsNotNullThrowsExceptionWithNullNullable()
        {
            Check.That((Mood?)null).IsNotNull();
        }

        [Test]
        public void IsNotNullDoesNotLoseOriginalTypeForOtherCheck()
        {
            var values = new[] { 0, 1, 2 };
            Check.That(values).IsNotNull().And.HasSize(3);
        }

        [Test]
        public void NotIsNullWorksWithNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).Not.IsNull();
        }

        [Test]
        public void NotIsNotNullWorksWithNullable()
        {
            Check.That((Mood?)null).Not.IsNotNull();
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value must be null.\nThe checked value:\n\t[NFluent.Tests.Extensions.Mood]")]
        public void NotIsNotNullThrowsWithNonNullNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).Not.IsNotNull();
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value is null whereas it must not.")]
        public void NotIsNullThrowsExceptionWithNullNullable()
        {
            Check.That((Mood?)null).Not.IsNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must be null.\nThe checked object:\n\t[System.Object]")]
        public void IsNullFailsProperly()
        {
            Check.That(new object()).IsNull();
        }

        [Test]
        public void IsNotNullWork()
        {
            Check.That(new object()).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must not be null.\nThe checked object:\n\t[null]")]
        public void IsNotNullFailsProperly()
        {
            Check.That((object)null).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must have be an instance distinct from expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: distinct from\n\t[System.Object]")]
        public void IsDistinctFailsProperly()
        {
            var test = new object();
            Check.That(test).IsDistinctFrom(test);
        }

        [Test]
        public void NegatedSameWorks()
        {
            var test = new object();
            Check.That(test).Not.IsDistinctFrom(test);
            Check.That(new object()).Not.IsSameReferenceAs(new object());
        }
    }
}
