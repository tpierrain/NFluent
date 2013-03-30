// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IntFluentAssertionExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Rui CARVALHO
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
namespace Spike
{
    using System;
    using NFluent;

    /// <summary>
    /// Provides assertion methods to be executed on an integer value.
    /// </summary>
    public static class IntFluentAssertionExtensions
    {
        /// <summary>
        /// Dummy method for spike purpose (to be deleted). Determines whether [is the ultimate question of life answer] [the specified fluent assertion].
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="System.Exception">The current value is not the answer to the ultimate question of life.</exception>
        public static ChainableFluentAssertion<IFluentAssertion<int>> IsTheUltimateQuestionOfLifeAnswer(this IFluentAssertion<int> fluentAssertion)
        {
            if (fluentAssertion.Value != 42)
            {
                throw new Exception("Not!!!! Try again!");
            }

            return new ChainableFluentAssertion<IFluentAssertion<int>>(fluentAssertion);
        }
    }
}