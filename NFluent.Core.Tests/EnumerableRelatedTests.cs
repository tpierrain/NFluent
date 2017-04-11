// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumerableRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
using System;

namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerableRelatedTests
    {
        private static readonly List<int> EmptyEnumerable = new List<int>();

        #region HasSize

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };

            Check.That(array).HasSize(4);
        }

        [Test]
        public void HasSizeGivesTheNumberOfElementsAndNotTheCapacity()
        {
            var enumerable = new List<string>(500);

            Check.That(enumerable).HasSize(0);
        }

        [Test]
        public void HasSizeWorksWithEnumerable()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4);
        }

        [Test]
        public void HasSizeWorksWithGenericEnumerable()
        {
            IEnumerable<int> enumerable = new List<int> { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4).And.Contains(666);
        }

#if !NETCOREAPP1_0
        [Test]
        public void HasSizeWorksWithArrayList()
        {
            var arrayList = new ArrayList { 45, 43, 54, 666 };

            Check.That(arrayList).HasSize(4);
        }
#endif

        [Test]
        public void HasSizeThrowsExceptionWhenFailingWithOneElementFound()
        {
            var enumerable = new List<int> { 666 };

            Check.ThatCode(() =>
            {
                Check.That(enumerable).HasSize(5);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable has 1 element instead of 5." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[666]");
        }

        [Test]
        public void NotHasSizeWorks()
        {
            var enumerable = new List<int> { 666 };
            
            Check.That(enumerable).Not.HasSize(5);
        }

        [Test]
        public void NotHasSizeThrowsExceptionWhenFailing()
        {
            var enumerable = new List<int> { 666 };

            Check.ThatCode(() =>
            {
                Check.That(enumerable).Not.HasSize(1);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable has 1 element which is unexpected." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[666]");
        }

        [Test]
        public void HasSizeThrowsExceptionWithClearStatusWhenFailsWithOneExpectedElement()
        {
            var enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() =>
            {
                Check.That(enumerable).HasSize(1);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable has 4 elements instead of 1." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[45, 43, 54, 666]");
        }

        #endregion

        #region IsEmpty

        [Test]
        public void IsEmptyWorks()
        {
            var emptyEnumerable = new List<int>();

            Check.That(emptyEnumerable).IsEmpty();
        }

        [Test]
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.ThatCode(() =>
            {
                Check.That(persons).IsEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable is not empty." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[null, null, Thomas]");
        }

        [Test]
        public void IsNullOrEmptyWorks()
        {
            Check.That(EmptyEnumerable).IsNullOrEmpty();
            Check.That((IEnumerable)null).IsNullOrEmpty();
        }

        [Test]
        public void IsNullOrEmptyFailsAppropriately()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };

            Check.ThatCode(() =>
            {
                Check.That(persons).IsNullOrEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable contains items, whereas it must be null or empty." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[null, null, Thomas]");
        }

        [Test]
        public void NotIsNullOrEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).Not.IsNullOrEmpty();
        }

        [Test]
        public void NotIsNullOrEmptyFailsIfEmpty()
        {
            Check.ThatCode(() =>
            {
                Check.That(EmptyEnumerable).Not.IsNullOrEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable is empty, where as it must contain at least one item.");
        }

        [Test]
        public void NotIsNullOrEmptyFailsIfNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((IEnumerable)null).Not.IsNullOrEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable is null, where as it must contain at least one item.");
        }

        [Test]
        public void NotIsEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.That(persons).Not.IsEmpty();
        }

        [Test]
        public void NotIsEmptyThrowsExceptionWhenFailing()
        {
            var persons = new List<Person>();

            Check.ThatCode(() =>
            {
                Check.That(persons).Not.IsEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable is empty, which is unexpected.");
        }

        #endregion

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() =>
            {
                Check.That(enumerable).Not.IsEqualTo(enumerable);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable is equal to the expected one whereas it must not." + Environment.NewLine + "The expected enumerable: different from" + Environment.NewLine + "\t[45, 43, 54, 666] of type: [System.Collections.Generic.List<int>]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() =>
            {
                Check.That(enumerable).Not.IsNotEqualTo(null);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[45, 43, 54, 666]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[null]");
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertion()
        {
            var killingSeries = new List<string> { "The wire", "Game of Thrones" };
            
            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);
            Check.That(killingSeries).IsEqualTo(killingSeries).And.Contains("The wire");
            Check.That(killingSeries).IsNotEqualTo(null).And.Contains("Game of Thrones");
            
            var integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertionOnEnumerable()
        {
            IEnumerable killingSeries = new List<string> { "The wire", "Game of Thrones" };

            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);

            IEnumerable integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }
    }
}