// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ContainsExactlyTests.cs" company="">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using System.IO;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ContainsExactlyTests
    {
        #region ContainsExactly with arrays

        [Test]
        public void ContainsExactlyWorksWithArrayOfInt()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(1, 2, 3, 4, 5, 666);
        }

        [Test]
        public void ContainsExactlyWorksWithArrayOfStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");
        }

        [Test]
        public void ContainsExactlyThrowsExceptionWhenMoreItemsAreIndicated()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };

            Check.ThatCode(() =>
            {
                Check.That(heroes).ContainsExactly("Luke", "Yoda", "Chewie", "Vader");
            })
            .IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain exactly the expected value(s). Elements are missing starting at index #3.",
                "The checked enumerable:", 
                "\t{\"Luke\",\"Yoda\",\"Chewie\"} (3 items)",
                "The expected value(s):",
                "\t{\"Luke\",\"Yoda\",\"Chewie\",*\"Vader\"*} (4 items)");
        }

        [Test]
        public void ContainsExactlyThrowsExceptionWhenItemsAreMissing()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };

            Check.ThatCode(() =>
            {
                Check.That(heroes).ContainsExactly("Luke", "Yoda");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain exactly the expected value(s). There are extra elements starting at index #2.",
                    "The checked enumerable:",
                    "\t{\"Luke\",\"Yoda\",*\"Chewie\"*} (3 items)",
                    "The expected value(s):",
                    "\t{\"Luke\",\"Yoda\"} (2 items)");
        }
        
        [Test]
        public void ContainsExactlyWithArraysThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };

            Check.ThatCode(() =>
            {
                Check.That(integers).ContainsExactly(666, 3, 1, 2, 4, 5);
            })
            .IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain exactly the expected value(s). First difference is at index #0.", 
                "The checked enumerable:",
                "\t{*1*,2,3,4,5,666} (6 items)",
                "The expected value(s):",
                "\t{*666*,3,1,2,4,5} (6 items)");

        }

        [Test]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };

            Check.ThatCode(() =>
            {
                Check.That(integers).ContainsExactly(42, 42, 42);
            })
            .IsAFailingCheckWithMessage("",
                                         "The checked enumerable does not contain exactly the expected value(s). First difference is at index #0.",
                                         "The checked enumerable:",
                                         "\t{*1*,2,3,4,5,666} (6 items)",
                                         "The expected value(s):",
                                         "\t{*42*,42,42} (3 items)");

        }

        [Test]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFailsWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };

            Check.ThatCode(() =>
                {
                    Check.That(guitarHeroes)
                        .ContainsExactly("Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell");
                })
                .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain exactly the expected value(s). First difference is at index #0.",
                    "The checked enumerable:",
                    "\t{*\"Hendrix\"*,\"Paco de Lucia\",\"Django Reinhardt\",\"Baden Powell\"} (4 items)",
                    "The expected value(s):",
                    "\t{\"Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell\"} (1 item)");

        }
        #endregion

        #region ContainsExactly with IEnumerable
        [Test]
        public void ContainsExactlyAlsoWorksWithEnumerableParameter()
        {
            Check.That(InstantiateDirectors().Extracting("Name")).ContainsExactly(InstantiateDirectors().Extracting("Name"));
        }

        [Test]
        public void ContainsExactlyThrowsWithEmptyList()
        {
            var emptyList = new List<int>();

            Check.ThatCode(() =>
            {
                Check.That(emptyList).ContainsExactly("what da heck!");
            })
            .IsAFailingCheckWithMessage("", 
                    "The checked enumerable does not contain exactly the expected value(s). Elements are missing starting at index #0.",
                     "The checked enumerable:", 
                    "\t{} (0 item)",
                    "The expected value(s):",
                    "\t{\"what da heck!\"} (1 item)");
        }

        [Test]
        public void ContainsExactlyDoNotThrowIfBothValuesAreEmptyLists()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).ContainsExactly(new List<int>());
        }

        [Test]
        public void ContainsExactlyThrowsWithNullAsCheckedValue()
        {
            Check.ThatCode(() =>
            {
                Check.That((List<int>) null).ContainsExactly("what da heck!");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable is null and thus does not contain exactly the expected value(s).",
                    "The expected value(s):",
                    "\t{\"what da heck!\"} (1 item)");
        }

        [Test]
        public void ContainsExactlyDoNotThrowIfBothValuesAreNull()
        {
            Check.That((List<int>) null).ContainsExactly(null);
        }

        [Test]
        public void ContainsExactlyWithEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 666 };
            IEnumerable expectedValues = new List<int> { 1, 3, 666, 2, 4, 5 };

            Check.ThatCode(() =>
            {
                Check.That(integers).ContainsExactly(expectedValues);
            })
            .IsAFailingCheckWithMessage("",
            "The checked enumerable does not contain exactly the expected value(s). First difference is at index #1.",
            "The checked enumerable:",
            "\t{1,*2*,3,4,5,666} (6 items)",
            "The expected value(s):",
            "\t{1,*3*,666,2,4,5} (6 items)");
        }

        [Test]
        public void ContainsExactlyWithEnumerableGenerateExtractForLongEnumerations()
        {
            var integers = new List<int>();
            var expected = new List<int>();
            for (var i = 0; i < 40; i++)
            {
                integers.Add(i);
                expected.Add(i);
            }
            integers[25] = 666;

            Check.ThatCode(() =>
            {
                Check.That(integers).ContainsExactly(expected);
            })
            .IsAFailingCheckWithMessage(
                "",
                "The checked enumerable does not contain exactly the expected value(s). First difference is at index #25.",
                "The checked enumerable:",
                "\t{...,15,16,17,18,19,20,21,22,23,24,*666*,26,27,28,29,30,31,32,33,34...} (40 items)",
                "The expected value(s):",
                "\t{...,15,16,17,18,19,20,21,22,23,24,*25*,26,27,28,29,30,31,32,33,34...} (40 items)");
        }

        [Test]
        public void ContainsExactlyFailsWithArrayOfArray()
        {
            var actual = new List<List<int>> {new List<int> {4}};
            var expected = new List<List<int>> {new List<int> {8}};
            Check.ThatCode(() => { Check.That(actual).IsEqualTo(expected); }).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is different from the expected one.",
                    "actual[0][0] = 4 instead of 8.",
                    "The checked enumerable:",
                    "\t{{4}} (1 item)",
                    "The expected enumerable:",
                    "\t{{8}} (1 item)");
        }

        [Test]
        public void ContainsExactlyFailsWithArrayOfArrayOfNull()
        {
            var actual = new List<List<object>> {new List<object> {4}};
            var expected = new List<List<object>> {new List<object> {null}};
            Check.ThatCode(() => { Check.That(actual).IsEqualTo(expected); }).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is different from the expected one.",
                    "actual[0][0] = 4 instead of null.",
                    "The checked enumerable:",
                    "\t{{4}} (1 item)",
                    "The expected enumerable:",
                    "\t{{null}} (1 item)");
        }

        [Test]
        public void ContainsExactlyWithGenericEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 666 };
            IEnumerable<int> expectedValues = new List<int> { 666, 3, 1, 2, 4, 5 };

            Check.ThatCode(() =>
            {
                Check.That(integers).ContainsExactly(expectedValues);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain exactly the expected value(s). First difference is at index #0.",
                    "The checked enumerable:",
                    "\t{*1*,2,3,4,5,666} (6 items)",
                    "The expected value(s):",
                    "\t{*666*,3,1,2,4,5} (6 items)");
        }

        [Test]
        public void ContainsExactlyThrowsExceptionWhenFailingWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Extracting("Name");

            Check.ThatCode(() =>
            {
                Check.That(InstantiateDirectors().Extracting("Name")).ContainsExactly(writersNames);
            })
            .IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain exactly the expected value(s). First difference is at index #0.",
                "The checked enumerable:",
                "\t{*\"Michel Gondry\"*,\"Joon-ho Bong\",\"Darren Aronofsky\"} (3 items)",
                "The expected value(s):",
                "\t{*\"Steve Tesich\"*,\"Albert Camus\",\"Eiji Yoshikawa\",\"Friedrich Nietzsche\"} (4 items)");
        }

        [Test]
        public void NotContainsExactlyWorksWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Extracting("Name");
            IEnumerable directorsNames = InstantiateDirectors().Extracting("Name");
            
            Check.That(directorsNames).Not.ContainsExactly(writersNames);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void NotContainsExactlyThrowsExceptionWhenFailingWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Extracting("Name");

            Check.ThatCode(() =>
            {
                Check.That(writersNames).Not.ContainsExactly(writersNames);
            })
            .IsAFailingCheckWithMessage("", 
                    "The checked enumerable contains exactly the given values whereas it must not.",
                    "The checked enumerable:",
                    "\t{\"Steve Tesich\",\"Albert Camus\",\"Eiji Yoshikawa\",\"Friedrich Nietzsche\"} (4 items)");
        }

        [Test]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithNullEnumerable()
        {
            Check.ThatCode(() =>
            {
                Check.That(InstantiateDirectors().Extracting("Name")).ContainsExactly(null);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable is not null whereas it should.",
                    "The checked enumerable:", 
                    "\t{\"Michel Gondry\",\"Joon-ho Bong\",\"Darren Aronofsky\"} (3 items)");
        }
        
        [Test]
        public void ContainsExactlyGenerateCorrectMessageWhenArrayWithOneItem()
        {
            Check.ThatCode(() =>
                    {
                        Check.That(new []{"test"}).ContainsExactly(null);
                    })
                .IsAFailingCheckWithMessage("",  
                    "The checked enumerable is not null whereas it should.", 
                    "The checked enumerable:", 
                    "\t{\"test\"} (1 item)");
        }

        [Test]
        public void ContainsExactlyWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            Check.That(variousObjects).ContainsExactly(expectedVariousObjects);
        }

        [Test]
        public void ContainsExactlyWorksOnLargeArrays()
        {
            if (!File.Exists(TestFiles.CheckedFile))
            {
                Assert.Ignore("Test File Not found");
            }
            var checkString = File.ReadAllBytes(TestFiles.CheckedFile);
            var expectedString = File.ReadAllBytes(TestFiles.CheckedFile);

            Check.That(checkString).ContainsExactly(expectedString);

        }
#endregion

#region test helpers

        private static IEnumerable<Person> InstantiateDirectors()
        {
            return new List<Person> 
                        {
                           new Person { Name = "Michel Gondry", Nationality = Nationality.French }, 
                           new Person { Name = "Joon-ho Bong", Nationality = Nationality.Korean }, 
                           new Person { Name = "Darren Aronofsky", Nationality = Nationality.American }
                       };
        }

        private static IEnumerable<Person> InstantiateWriters()
        {
            return new List<Person> 
                        {
                           new Person { Name = "Steve Tesich", Nationality = Nationality.Serbian }, 
                           new Person { Name = "Albert Camus", Nationality = Nationality.French }, 
                           new Person { Name = "Eiji Yoshikawa", Nationality = Nationality.Japanese }, 
                           new Person { Name = "Friedrich Nietzsche", Nationality = Nationality.German }
                       };
        }

#endregion
    }
}