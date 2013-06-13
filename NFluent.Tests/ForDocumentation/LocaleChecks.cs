﻿// // --------------------------------------------------------------------------------------------------------------------
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
namespace NFluent.Tests.ForDocumentation
{
    using NUnit.Framework;

    [TestFixture]
    public class LocaleChecks
    {
        [Test]
        [SetCulture("es-ES")]
        [Explicit("Scan all assemblies, execute tests in Spanish.")]
        public void Spanish()
        {
            RunnerHelper.RunAllTests(false);
        }

        [Test]
        [SetCulture("zh-CN")]
        [Explicit("Scan all assemblies, execute tests in Chinese.")]
        public void Chinese()
        {
            RunnerHelper.RunAllTests(false);
        }

        [Test]
        [SetCulture("fr-CA")]
        [Explicit("Scan all assemblies, execute tests in Canadian French.")]
        public void CanadianFrench()
        {
            RunnerHelper.RunAllTests(false);
        }

        [Test]
        [SetCulture("ja-JP")]
        [Explicit("Scan all assemblies, execute tests in Japanese.")]
        public void Japanese()
        {
            RunnerHelper.RunAllTests(false);
        }

        // TODO: makes the teamcity build execute Explicit tests
    }
}