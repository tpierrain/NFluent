// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ReflectionWrapperTests.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using System.Reflection;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    internal class ReflectionWrapperTests
    {
        [Test]
        public void HashCodeIsConsistent()
        {
            var toto = new {entier = 2, lettre = 'b', sub = new { sub = (object) null} };
            var sut = ReflectionWrapper.BuildFromInstance(toto.GetType(), toto,
                new Criteria(BindingFlags.Instance | BindingFlags.NonPublic, true, true));
            Check.That(sut.GetHashCode()).IsEqualTo(147721457);
        }
    }
}