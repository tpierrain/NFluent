// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ToEnumeratedStringTests.cs" company="">
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
    using System.Collections;

    using NUnit.Framework;

    [TestFixture]
    public class ToEnumeratedStringTests
    {
        [Test]
        public void ToEnumeratedStringParticularBehaviourWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes.ToEnumeratedString()).IsEqualTo(@"""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""");
        }

        [Test]
        public void ToEnumeratedStringWorksFineWithStrings()
        {
            var departments = new[] { 93, 56, 35, 75 };
            Check.That(departments.ToEnumeratedString()).IsEqualTo("93, 56, 35, 75");
        }

        [Test]
        public void ToEnumeratedStringWorksWithAnotherSeparator()
        {
            var departments = new[] { 93, 56, 35, 75 };

            Check.That(departments.ToEnumeratedString("|")).IsEqualTo("93|56|35|75");
        }

#if !NETCOREAPP1_0
        [Test]
        public void HowToEnumeratedStringHandlesNull()
        {
            var variousStuffs = new ArrayList { 93, null, "hell yeah!" };

            Check.That(variousStuffs.ToEnumeratedString()).IsEqualTo(@"93, null, ""hell yeah!""");
        }
#endif

        [Test]
        public void HowToEnumeratedStringHandlesNullEnumeration()
        {
            Check.That(((IEnumerable)null).ToEnumeratedString()).IsEqualTo("null");
        }
    }
}