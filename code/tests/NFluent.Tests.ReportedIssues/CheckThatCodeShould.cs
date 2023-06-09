// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CheckThatCodeShould.cs" company="NFluent">
//   Copyright 2023 Cyrille DUPUYDAUBY
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

using NUnit.Framework;

namespace NFluent.NetCore3.Tests.ReportedIssues
{
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class CheckThatCodeShould
    {
        [Test]
        public void Async_issue()
        {
            // simulates the code under test
            async Task<bool> PleaseThrowAsync()
            { 
                await Task.Yield();
                throw new InvalidOperationException("Oh dear!)");
            }
            async Task PleaseThrowAsync2()
            { 
                await Task.Yield();
                throw new InvalidOperationException("Oh dear!)");
            }

            Task PleaseThrowAsync3()
            {
                return Task.Run(() => throw new InvalidOperationException());
            }
            
            // assert
            //Check.ThatCode(async () => await PleaseThrowAsync()).Throws<InvalidOperationException>();
            Check.ThatCode(() => PleaseThrowAsync()).Throws<InvalidOperationException>();

            //Check.ThatCode(async () => await PleaseThrowAsync2()).Throws<InvalidOperationException>();
            //Check.ThatCode(() => PleaseThrowAsync2()).Throws<InvalidOperationException>();

            //Check.ThatCode(PleaseThrowAsync2).Throws<InvalidOperationException>();
            Check.ThatCode(PleaseThrowAsync).Throws<InvalidOperationException>();
            SimpleTest(() => PleaseThrowAsync2());
        }

        void SimpleTest(Func<Task> attempt)
        {

        }
    }
}