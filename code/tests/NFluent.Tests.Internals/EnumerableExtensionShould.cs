// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnumerableExtensionShould.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerableExtensionShould
    {
        [Test]
        public void CastFromAnyType()
        {
            var numbers = new ArrayList {1, 2, 3, 4};

            var enumerable = numbers.AmbitiousCast<int>();

            Check.That(enumerable).IsNotNull();

            var iterator = enumerable.GetEnumerator();
            Check.That(iterator).IsNotNull();
            Check.That(iterator.MoveNext()).IsTrue();
            Check.That(iterator.Current).IsEqualTo(1);
            iterator.Reset();

            Check.That(iterator.MoveNext()).IsTrue();
            Check.That(iterator.Current).IsEqualTo(1);

            Check.That(((IEnumerator) iterator).Current).IsEqualTo(1);

            iterator.Dispose();
        }
    }
}