// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectRelatedTest.cs" company="">
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
    using NUnit.Framework;

    [TestFixture]
    public class ObjectRelatedTest
    {
        [Test]
        public void IsSameObjecWorks()
        {
            var test = new object();
            Check.That(test).IsSameReferenceThan(test);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked object must be the same instance than expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: same instance than\n\t[System.Object]")]
        public void IsSameReferenceFailsProperly()
        {
            Check.That(new object()).IsSameReferenceThan(new object());
        }

        [Test]
        public void IsDistinctWorks()
        {
            Check.That(new object()).IsDistinctFrom(new object());
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked object must have be an instance distinct from expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: distinct from\n\t[System.Object]")]
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
            Check.That(new object()).Not.IsSameReferenceThan(new object());           
        }
    }
}
