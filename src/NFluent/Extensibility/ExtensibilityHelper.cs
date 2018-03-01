// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtensibilityHelper.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent.Extensibility
{
    using Kernel;

    /// <summary>
    /// Helper that allow to extract the checker to be used for and from any given fluent check instance.
    /// </summary>
    public static class ExtensibilityHelper
    {
        /// <summary>
        /// Extracts the checker to be used in order to check things on the value contained within
        /// the given fluent check.
        /// </summary>
        /// <typeparam name="T">The type of checked value.</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>
        /// The checker to be used to check things on the value contained in the fluent check.
        /// </returns>
        public static IChecker<T, ICheck<T>> ExtractChecker<T>(ICheck<T> check)
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            // ReSharper disable once SuspiciousTypeConversion.Global
            return ((ICheckForExtensibility<T, ICheck<T>>)check).Checker;
        }

        /// <summary>
        /// Extracts the code checker.
        /// </summary>
        /// <typeparam name="TU">The type of checked value.</typeparam>
        /// <param name="check">The check.</param>
        /// <returns>The checker to be used to check things on the value contained in the fluent check.</returns>
        public static IChecker<TU, ICodeCheck<TU>> ExtractCodeChecker<TU>(ICodeCheck<TU> check) where TU : RunTrace
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            // ReSharper disable once SuspiciousTypeConversion.Global
            return ((ICheckForExtensibility<TU, ICodeCheck<TU>>)check).Checker;
        }

        /// <summary>
        /// Extracts the checker to be used in order to check things on the struct instance contained within
        /// the given fluent check.
        /// </summary>
        /// <typeparam name="TS">The type of the struct to be checked.</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>
        /// The checker to be used to check things on the value contained in the fluent check.
        /// </returns>
        public static IChecker<TS, IStructCheck<TS>> ExtractStructChecker<TS>(IStructCheck<TS> check) where TS : struct
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            // ReSharper disable once SuspiciousTypeConversion.Global
            return ((ICheckForExtensibility<TS, IStructCheck<TS>>)check).Checker;
        }

        /// <summary>
        /// Gets an <see cref="IExtendableCheckLink{T, TU}"/> that permits refining checks
        /// </summary>
        /// <param name="check">check to extend</param>
        /// <param name="value">initial operands</param>
        /// <typeparam name="T">type of checked value</typeparam>
        /// <typeparam name="TU">Type of comparand for previous check</typeparam>
        /// <returns>An <see cref="IExtendableCheckLink{T,TU}"/>implementation.</returns>
        public static IExtendableCheckLink<T, TU> BuildExtendableCheckLink<T, TU>(ICheck<T> check, TU value)
        {
            return new ExtendableCheckLink<T, TU>(check, value);
        }

        /// <summary>
        /// Initiates a check logic chain.
        /// </summary>
        /// <typeparam name="T">Type of sut</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <param name="inverted">set to true to invert checking logic.</param>
        /// <returns>An <see cref="ICheckLogic{T}"/>instance.</returns>
        public static ICheckLogic<T> BeginCheck<T>(ICheck<T> check, bool inverted = false)
        {
            return ExtractChecker(check).BeginCheck(inverted);
        }

        /// <summary>
        /// Builds a chainable object.
        /// </summary>
        /// <typeparam name="T">Type of sut</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>An <see cref="ICheckLink{T}"/> instance to add further checks.</returns>
        public static ICheckLink<ICheck<T>> BuildCheckLink<T>(ICheck<T> check)
        {
            return ExtractChecker(check).BuildChainingObject();
        }
    }
}