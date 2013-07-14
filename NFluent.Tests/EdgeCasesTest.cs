﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EdgeCasesTest.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
    public class EdgeCasesTest
    {
        [Test]
        public void IsInstanceOfCanBeLinked()
        {
            // syntax ok
            Check.That(DateTime.Now).IsInstanceOf<DateTime>().And.IsEqualToIgnoringHours(DateTime.Now);

            // fails with nullrefexception
            Check.That(new Person()).IsInstanceOf<Person>();

            Check.That((int?)null).IsInstanceOf<int?>();
        }

        [Test]
        public void IsATest()
        {
            ////Check.That(new object()).IsA<object>();
            ////Check.That(new object()).IsA<int>(); // fails
        }
    }
}
