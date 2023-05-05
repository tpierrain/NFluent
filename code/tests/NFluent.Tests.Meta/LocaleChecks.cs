// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LocaleChecks.cs" company="">
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
// ReSharper disable once CheckNamespace

using NFluent.Tests.ForDocumentation;

namespace NFluent.Tests
{
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class LocaleChecks
    {
        [Test]
        [Explicit]
        public void Spanish()
        {
            var assembly = typeof(ObjectRelatedTest).GetTypeInfo().Assembly;
            using (new CultureSession("es-ES"))
            {
                RunnerHelper.RunAllTests(assembly, false);
            }
        }

        [Test]
        [Explicit]
        public void Chinese()
        {
            var assembly = typeof(ObjectRelatedTest).GetTypeInfo().Assembly;
            using (new CultureSession("zh-CN"))
            {
                RunnerHelper.RunAllTests(assembly, false);
            }
        }

        [Test]
        [Explicit]
        public void CanadianFrench()
        {
            var assembly = typeof(ObjectRelatedTest).GetTypeInfo().Assembly;
            using (new CultureSession("fr-CA"))
            {
                RunnerHelper.RunAllTests(assembly, false);
            }
        }

        [Test]
        [Explicit]
        public void Japanese()
        {
            var assembly = typeof(ObjectRelatedTest).GetTypeInfo().Assembly;
            using (new CultureSession("ja-JP"))
            {
                RunnerHelper.RunAllTests(assembly, false);
            }
        }
    }
}