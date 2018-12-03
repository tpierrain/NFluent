// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ICheckLogic.cs" company="NFluent">
//   Copyright 2018 Cyrille DUPUYDAUBY
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
#if !DOTNET_35 && !DOTNET_20 && !DOTNET_30
    using System;
#endif
    /// <summary>
    /// Hosts extensions methods for IChecKLogic
    /// </summary>
    public static class CheckLogicExtensions
    {
        private const string CanTBeUsedWhenNegated = "{0} can't be used when negated";

        /// <summary>
        /// Failing condition
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="predicate">Predicate, returns true if test fails.</param>
        /// <param name="error">Error message on failure</param>
        /// <param name="newOptions"></param>
        /// <returns>Continuation object.</returns>
        public static ICheckLogic<T> FailWhen<T>(this ICheckLogic<T> logic, Func<T, bool> predicate, string error, MessageOption newOptions = MessageOption.None)
        {
            return logic.Analyze((sut2, test) =>
            {
                if (predicate(sut2))
                {
                    test.Fail(error, newOptions);
                }
            });
        }

        /// <summary>
        /// Fails the check is the checked value is null,
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="error">Error message</param>
        /// <typeparam name="T">type of the checked object</typeparam>
        /// <returns>Continuation object.</returns>
        public static ICheckLogic<T> FailIfNull<T>(this ICheckLogic<T> logic, string error = "The {0} is null.")
        {
            return logic.FailWhen(sut => sut == null, error, MessageOption.NoCheckedBlock);
        }

        /// <summary>
        /// Error message for negated checks.
        /// </summary>
        /// <param name="logic">check</param>
        /// <param name="message">Message template to use when check succeeds.</param>
        /// <param name="option"></param>
        /// <typeparam name="T">type of the checked object</typeparam>
        /// <returns>Continuation object.</returns>
        public static ICheckLogic<T> OnNegate<T>(this ICheckLogic<T> logic, string message, MessageOption option = MessageOption.None)
        {
            return logic.OnNegateWhen(_ => true, message, option);
        }

        /// <summary>
        /// Generate an error message stating that this check cannot be used with <see cref="INegateableCheck{T}.Not"/>
        /// </summary>
        /// <typeparam name="T">type of the checked object</typeparam>
        /// <param name="logic">check</param>
        /// <param name="checkName">name of the source check</param>
        /// <returns>Continuation object.</returns>
        public static ICheckLogic<T> CantBeNegated<T>(this ICheckLogic<T> logic, string checkName)
        {
            var message = string.Format(CanTBeUsedWhenNegated, checkName);
            if (logic.IsNegated)
                throw  new InvalidOperationException(message);
            logic.OnNegate(message);
            return logic;
        }
    }
}