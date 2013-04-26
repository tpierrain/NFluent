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
namespace NFluent.Tests.ForDocumentation
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class LocaleChecks
    {
        private CultureInfo original; 
        
        [TestFixtureSetUp]
        public void SaveLocal()
        {
            this.original = Thread.CurrentThread.CurrentCulture;
        }

        [TestFixtureTearDown]
        public void RestoreLocal()
        {
            Thread.CurrentThread.CurrentCulture = this.original;
        }

        [Test]
        [Explicit("Scan all assemblies, execute tests in Spanish.")]
        public void Spanish()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES", false);
            RunnerHelper.RunAllTests();
        }

        [Test]
        public void Chinese()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN", false);
            RunnerHelper.RunAllTests();
        }

        [Test]
        public void CanadianFrench()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA", false);
            RunnerHelper.RunAllTests();
        }

        [Test]
        [Explicit("Scan all assemblies, execute tests in Japanese.")]
        public void Japanese()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP", false);
            RunnerHelper.RunAllTests();
        }
    }
}