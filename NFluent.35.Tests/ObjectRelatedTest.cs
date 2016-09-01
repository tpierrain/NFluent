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
        public void IsSameReferenceThanWorks()
        {
            var test = new object();
            Check.That(test).IsSameReferenceThan(test);
        }

        [Test]
        public void IsSameReferenceThanDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new[] { 1, 4, 42 };
            Check.That(numbers).IsSameReferenceThan(numbers).And.Contains(42);
        }

        [Test]
        public void IsSameReferenceFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(new object()).IsSameReferenceThan(new object());
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked object must be the same instance than expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: same instance than\n\t[System.Object]");
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
        public void IsNotNullThrowsExceptionWithNullNullable()
        {
            Check.ThatCode(() =>
            {
                Check.That((Mood?)null).IsNotNull();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable value is null whereas it must not.");
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
        public void NotIsNotNullThrowsWithNonNullNullable()
        {
            Mood? goodMood = new Mood();

            Check.ThatCode(() =>
            {
                Check.That(goodMood).Not.IsNotNull();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable value must be null.\nThe checked value:\n\t[NFluent.Tests.Extensions.Mood]");
        }
        
        [Test]
        public void NotIsNullThrowsExceptionWithNullNullable()
        {
            Check.ThatCode(() =>
            {
                Check.That((Mood?)null).Not.IsNull();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable value is null whereas it must not.");
        }

        [Test]
        public void IsNullFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(new object()).IsNull();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked object must be null.\nThe checked object:\n\t[System.Object]");
        }

        [Test]
        public void IsNotNullWork()
        {
            Check.That(new object()).IsNotNull();
        }

        [Test]
        public void IsNotNullFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That((object)null).IsNotNull();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked object must not be null.\nThe checked object:\n\t[null]");
        }

        [Test]
        public void IsDistinctFailsProperly()
        {
            var test = new object();

            Check.ThatCode(() =>
            {
                Check.That(test).IsDistinctFrom(test);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked object must have be an instance distinct from expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: distinct from\n\t[System.Object]");
        }

        [Test]
        public void NegatedSameWorks()
        {
            var test = new object();
            Check.That(test).Not.IsDistinctFrom(test);
            Check.That(new object()).Not.IsSameReferenceThan(new object());
        }
    }
}
