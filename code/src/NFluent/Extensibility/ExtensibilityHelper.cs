// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensibilityHelper.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent.Extensibility
{
    using System;
    using Kernel;
    using Messages;

    /// <summary>
    /// Helper that allow to extract the checker to be used for and from any given fluent check instance.
    /// </summary>
    public static class ExtensibilityHelper
    {
        /// <summary>
        /// Extracts the checker to be used to check things on the value contained within
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
        /// Gets an <see cref="IExtendableCheckLink{T, TU}"/> that permits refining checks
        /// </summary>
        /// <param name="check">check to extend</param>
        /// <param name="value">initial operands</param>
        /// <typeparam name="T">type of checked value</typeparam>
        /// <typeparam name="TU">Type of comparand for previous check</typeparam>
        /// <returns>An <see cref="IExtendableCheckLink{T,TU}"/>implementation.</returns>
        public static ExtendableCheckLink<T, TU> BuildExtendableCheckLink<T, TU>(T check, TU value) where T: class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            return new ExtendableCheckLink<T, TU>(check, value);
        }

        /// <summary>
        /// Initiates a check logic chain.
        /// </summary>
        /// <typeparam name="T">Type of sut</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>An <see cref="ICheckLogic{T}"/>instance.</returns>
        public static ICheckLogic<T> BeginCheck<T>(ICheck<T> check)
        {
            return ExtractChecker(check).BeginCheck();
        }

        /// <summary>
        /// Initiates a check logic chain.
        /// </summary>
        /// <typeparam name="T">Type of sut</typeparam>
        /// <typeparam name="TU">Target type</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <param name="converter">Conversion method </param>
        /// <returns>An <see cref="ICheckLogic{T}"/>instance.</returns>
        public static ICheckLogic<TU> BeginCheckAs<T, TU>(ICheck<T> check, Func<T, TU> converter) where TU: class
        {
            var toConvert = ExtractChecker(check).Value;
            ICheck<TU> altCheck = new FluentCheck<TU>(toConvert == null ? null : converter(toConvert));
            if (ExtractChecker(check).Negated)
            {
                altCheck = altCheck.Not;
            }
            return ExtractChecker(altCheck).BeginCheck();
        }

        /// <summary>
        /// Initiates a check logic chain.
        /// </summary>
        /// <typeparam name="T">Type of sut</typeparam>
        /// <param name="sut">The fluent check instance to work on.</param>
        /// <returns>An <see cref="ICheckLogic{T}"/>instance.</returns>
        public static ICheckLogic<T> BeginCheck<T>(FluentSut<T> sut)
        {
            return new CheckLogic<T>(sut);
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

        /// <summary>
        /// Builds a chainable check with a sub item.
        /// </summary>
        /// <param name="check">original check to link to</param>
        /// <param name="item">sub item that can be check with which</param>
        /// <param name="label">label for the sub item</param>
        /// <typeparam name="TU">type of the sut</typeparam>
        /// <typeparam name="T">type of the sub item</typeparam>
        /// <returns>A chainable link supporting Which</returns>
        public static ICheckLinkWhich<ICheck<TU>, ICheck<T>> BuildCheckLinkWhich<TU, T>(ICheck<TU> check, T item, EntityNamingLogic label)
        {
            var chk = new FluentCheck<T>(item);
            chk.SutName.Merge(label);
            return new CheckLinkWhich<ICheck<TU>, ICheck<T>>(check, chk);
        }

        /// <summary>
        /// Builds a chainable check with a sub item.
        /// </summary>
        /// <typeparam name="TU">type of the sut</typeparam>
        /// <typeparam name="T">type of the sub item</typeparam>
        /// <param name="check">original check to link to</param>
        /// <param name="item">sub item that can be check with which</param>
        /// <param name="label">label for the sub item</param>
        /// <param name="hasItem">set to false is no item is available</param>
        /// <returns>A chainable link supporting Which</returns>
        public static ICheckLinkWhich<ICheck<TU>, ICheck<T>> BuildCheckLinkWhich<TU, T>(ICheck<TU> check, T item, string label, bool hasItem = true)
        {
            FluentCheck<T> chk = null;
            if (hasItem)
            {
                chk = new FluentCheck<T>(item);
                if (!string.IsNullOrEmpty(label))
                {
                    chk.Checker.SetSutLabel(label);
                }
            }

            return new CheckLinkWhich<ICheck<TU>, ICheck<T>>(check, chk);
        }
    }
}