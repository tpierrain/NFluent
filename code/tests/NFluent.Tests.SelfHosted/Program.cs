// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Program.cs" company="NFluent">
//   Copyright 2019 Cyrille DUPUYDAUBY & Thomas PIERRAIN
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

namespace NFluent.Tests.SelfHosted
{
    using System;
    using Extensibility;
    using Helpers;
    using Kernel;
    using NFluent;

    /// <summary>
    ///     Performs some test to simulate unknown test framework
    /// </summary>
    public class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                BasicTest();
                ExceptionScanTest();
                AssumptionTest();
                HandleProperlyNegationOnFailing();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }

        private static void AssumptionTest()
        {
            
            Check.ThatCode(() => Assuming.That(2).IsEqualTo(3)).Throws<FluentCheckException>();
            Check.ThatCode(() => Assuming.That(2).IsEqualTo(3)).IsAFailingAssumption();
        }

        public static void BasicTest()
        {
            Check.That("MsTest").IsNotEmpty();

            Check.ThatCode(() => Check.That("MsTest").IsEqualTo("great")).Throws<FluentCheckException>();
        }

        public static void ExceptionScanTest()
        {
            Check.That(ExceptionHelper.BuildException("Test")).IsInstanceOf<FluentCheckException>();
        }

        public static void HandleProperlyNegationOnFailing()
        {
            var check = Check.That(2);
            var checker = ExtensibilityHelper.ExtractChecker(check.Not);
            
            checker.ExecuteNotChainableCheck(()=> throw ExceptionHelper.BuildException("oups"), "should have failed");
            Check.ThatCode(() =>
                    checker.ExecuteNotChainableCheck(() => { }, "should have failed"))
                .IsAFailingCheck();
        }

    }
}