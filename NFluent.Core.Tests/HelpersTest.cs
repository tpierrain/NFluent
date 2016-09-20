// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HelpersTest.cs" company="">
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
    using NFluent.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class HelpersTest
    {
        [Test]
        public void TestIsNullable()
        {
            Check.That(typeof(int?).IsNullable()).IsTrue();
            Check.That(typeof(int).IsNullable()).IsFalse();
            Check.That(typeof(object).IsNullable()).IsFalse();
        }

        [Test]
        public void TestImplementEquals()
        {
            Check.That(typeof(object).ImplementsEquals()).IsTrue();
            Check.That(typeof(HelpersTest).ImplementsEquals()).IsFalse();
            Check.That(typeof(MyDummyClass).ImplementsEquals()).IsTrue();
        }

        private class MyDummyClass 
        {
            public override bool Equals(object other)
            {
                return true;
            }

            /// <summary>
            /// Serves as a hash function for a particular type. 
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object"/>.
            /// </returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
