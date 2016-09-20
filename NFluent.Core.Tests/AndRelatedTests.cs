// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="AndRelatedTests.cs" company="">
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
    using NUnit.Framework;

    [TestFixture]
    public class AndRelatedTests
    {
        [Test]
        public void CanUseAndInAnyOrderForAllFluentStringAssertOperations()
        {
            const string Heroes = "Batman and Robin";

            Check.That(Heroes).Not.Contains("Joker").And.StartsWith("Bat").And.Contains("Robin");

            Check.That(Heroes).Contains("and").And.IsInstanceOf<string>().And.IsNotInstanceOf<Person>().And.IsNotEqualTo(null);
            Check.That(Heroes).Contains("Robin").And.StartsWith("Batman").And.IsInstanceOf<string>();
            Check.That(Heroes).IsInstanceOf<string>().And.Contains("Batman", "Robin").And.StartsWith("Batm");
            Check.That(Heroes).Contains("and").And.IsNotInstanceOf<Person>().And.IsInstanceOf<string>();
        }
    }
}
