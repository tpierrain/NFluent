// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ILambdaAssertion.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

namespace NFluent
{
    using NFluent.Helpers;

    /// <summary>
    /// Provides lambda/action specific check.
    /// </summary>
    public interface ILambdaAssertion : IForkableCheck
    {
        /// <summary>
        /// Checks that the execution time is below a specified threshold.
        /// </summary>
        /// <param name="threshold">The threshold.</param>
        /// <param name="timeUnit">The time unit of the given threshold.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">Execution was strictly above limit.</exception>
        IChainableCheck<ILambdaAssertion> LastsLessThan(double threshold, TimeUnit timeUnit);

        /// <summary>
        /// Check that the code does not throw an exception.
        /// </summary>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The code raised an exception.</exception>
        IChainableCheck<ILambdaAssertion> DoesNotThrow();

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <typeparam name="T">And.Expected exception type.</typeparam>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of the specified type, or did not raised an exception at all.</exception>
        IChainableCheck<ILambdaAssertion> Throws<T>();

        /// <summary>
        /// Checks that the code did throw an exception of any type.
        /// </summary>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        IChainableCheck<ILambdaAssertion> ThrowsAny();
    }
}