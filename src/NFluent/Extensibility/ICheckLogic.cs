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
#if !DOTNET_35
    using System;
#endif
    /// <summary>
    /// Options for message generation.
    /// </summary>
    [System.Flags]
    public enum MessageOption
    {
        /// <summary>
        /// No specific option
        /// </summary>
        None = 0,
        /// <summary>
        /// Removes the description block for the checked value or sut
        /// </summary>
        NoCheckedBlock = 1,
        /// <summary>
        /// Removes the description block for the expected value(s)
        /// </summary>
        NoExpectedBlock
    }

    /// <summary>
    ///     Provides method to ease coding of checks.
    /// </summary>
    public interface ICheckLogic<out T>
    {
        /// <summary>
        /// Failing condition
        /// </summary>
        /// <param name="predicate">Predicate, returns true if test fails.</param>
        /// <param name="error">Error message on failure</param>
        /// <param name="option"></param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> FailsIf(Func<T, bool> predicate, string error, MessageOption option = MessageOption.None);

        /// <summary>
        ///     Ends check.
        /// </summary>
        /// <remarks>At this point, exception is thrown.</remarks>
        void EndCheck();

        /// <summary>
        ///     Specify expected value.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparison"></param>
        /// <param name="negatedComparison"></param>
        /// <param name="label"></param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> Expecting<TU>(TU other, string comparison = null, string negatedComparison = null, string label = null);

        /// <summary>
        /// Error message for negated checks.
        /// </summary>
        /// <param name="message">Message template to use when check succeeds.</param>
        /// <returns></returns>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> Negates(string message);

        /// <summary>
        /// Failing condition on check negation.
        /// </summary>
        /// <param name="predicate">Predicate, returns true if test fails.</param>
        /// <param name="error">Error message on failure</param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> NegatesIf(Func<T, bool> predicate, string error);

    }
}