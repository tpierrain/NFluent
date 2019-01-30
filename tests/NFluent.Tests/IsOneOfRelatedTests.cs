// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsOneOfRelatedTests.cs" company="">
//   Copyright 2017 Thomas PIERRAIN, Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class IsOneOfRelatedTests
    {
        [Test]
        public void ShouldSucceedIfValueIsAmongLegalOnes()
        {
            Check.That("foo").IsOneOf("foo", "bar", "foobar");
            Check.That("bar").IsOneOf("foo", "bar", "foobar");
            Check.That("foobar").IsOneOf("foo", "bar", "foobar");
        }

        [Test]
        public void ShouldSucceedWhenNegated()
        {
            Check.That("fu").Not.IsOneOf("foo", "bar", "foobar");
        }

        [Test]
        public void ShouldFailIfValueIsNotAmongLegalOnes()
        {
            Check.ThatCode( () =>
            {
                Check.That("fu").IsOneOf("foo", "bar", "foobar");
            }).IsAFailingCheckWithMessage("",
                "The checked string is not one of the expected value(s).", 
            "The checked string:", 
            "\t[\"fu\"]",
            "The expected value(s): one of", 
            "\t{\"foo\", \"bar\", \"foobar\"} (3 items)");
        }

        [Test]
        public void ShouldFailIfWhenNegate()
        {
            Check.ThatCode( () =>
            {
                Check.That("foo").Not.IsOneOf("foo", "bar", "foobar");
            }).IsAFailingCheckWithMessage("",
                "The checked string should not be one of the expected value(s).", 
            "The checked string:", 
            "\t[\"foo\"]",
            "The expected value(s): none of", 
            "\t{\"foo\", \"bar\", \"foobar\"} (3 items)");
        }
    }
}