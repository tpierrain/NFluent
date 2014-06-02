// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="UserReportedIssuesTests.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY
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
    public class UserReportedIssuesTests
    {
        public interface IModelBName
        {
            string Title { get; set; }
        }

        [Test]
        public void NullRefExcOnEnumerables()
        {
            var values = new[] { "Yoda", null };
            Check.That(values).Contains(null);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException))]
        public void NullRefonHasFieldsWithSameValueWithInterfaces()
        {
            var modelA = new ModelA { Name = "Yoda" };
            var modelB = new ModelB { Name = new ModelBName { Title = "Frank" } };

            Check.That(modelA).HasFieldsWithSameValues(modelB);
        }

        // 30/05/14 Invalid exception on strings with curly braces
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain the expected value(s):\n\t[\"MaChaine{964}\"]\nThe checked enumerable:\n\t[\"MaChaine{94}\"]\nThe expected value(s):\n\t[\"MaChaine{964}\"]")]
        public void SpuriousExceptionOnError()
        {
            var toTest = new System.Collections.ArrayList { "MaChaine{94}" };
            string result = "MaChaine{964}";
            Check.That(toTest).Contains(result);
        }

        public class ModelA
        {
            public string Name { get; set; }
        }

        public class ModelB
        {
            public IModelBName Name { get; set; }
        }

        public class ModelBName : IModelBName
        {
            public string Title { get; set; }
        }
    }
}