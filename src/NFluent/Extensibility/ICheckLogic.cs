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
    using System.Collections;

#if !DOTNET_35 && !DOTNET_20 && !DOTNET_30
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
        NoExpectedBlock = 2,
        /// <summary>
        /// Forces the sut type
        /// </summary>
        ForceType = 4,
        /// <summary>
        /// Add type info
        /// </summary>
        WithType = 8,
        /// <summary>
        /// Add hash for values
        /// </summary>
        WithHash = 16
    }

    /// <summary>
    /// Minimal interface
    /// </summary>
    public interface ICheckLogicBase
    {
        /// <summary>
        ///     Ends check.
        /// </summary>
        /// <remarks>At this point, exception is thrown.</remarks>
        /// <returns>true if succesfull</returns>
        void EndCheck();

        /// <summary>
        /// Gets the failed status.
        /// </summary>
        bool Failed { get; }
    }

    /// <summary>
    /// Provides method to ease coding of checks.
    /// </summary>
    public interface ICheckLogic<out T> : ICheckLogicBase
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
        /// Failing condition
        /// </summary>
        /// <param name="predicate">Predicate, returns true if test fails.</param>
        /// <param name="errorBuilder">Error message builder function</param>
        /// <param name="option"></param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> FailsIf(Func<T, bool> predicate, Func<T, ICheckLogic<T>, string> errorBuilder, MessageOption option = MessageOption.None);

        /// <summary>
        ///     Specify expected value.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparison"></param>
        /// <param name="negatedComparison"></param>
        /// <param name="expectedLabel"></param>
        /// <param name="negatedLabel"></param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> Expecting<TU>(TU other, string comparison = null, string negatedComparison = null, string expectedLabel = null, string negatedLabel = null);

        /// <summary>
        /// Error message for negated checks.
        /// </summary>
        /// <param name="message">Message template to use when check succeeds.</param>
        /// <param name="option"></param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> Negates(string message, MessageOption option = MessageOption.None);

        /// <summary>
        /// Failing condition on check negation.
        /// </summary>
        /// <param name="predicate">Predicate, returns true if test fails.</param>
        /// <param name="error">Error message on failure</param>
        /// <param name="option">Options to use on parts of the message</param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> NegatesIf(Func<T, bool> predicate, string error, MessageOption option = MessageOption.None);

        /// <summary>
        /// Executes arbitrary code on the sut.
        /// </summary>
        /// <param name="action">Code to be executed</param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> Analyze(Action<T, ICheckLogic<T>> action);

        /// <summary>
        /// Fails the check is the checked value is null,
        /// </summary>
        /// <param name="error">Error message</param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> FailsIfNull(string error = "The {0} is null.");

        /// <summary>
        /// Raises an InvalidOperationException if predicte returns true. 
        /// </summary>
        /// <param name="predicate">predicate function</param>
        /// <param name="error">error message in exception</param>
        /// <returns>Continuation object.</returns>
        ICheckLogic<T> InvalidIf(Func<T, bool> predicate, string error);
        
        /// <summary>
        /// Set the name for the observed system.
        /// </summary>
        /// <param name="name">Name to use</param>
        /// <returns>Continuation object</returns>
        ICheckLogic<T> SutNameIs(string name);

        /// <summary>
        /// Change the value of the sut.
        /// </summary>
        /// <param name="sutExtractor">new sut  object.</param>
        /// <param name="sutLabel">new label</param>
        /// <returns>Continuation object</returns>
        ICheckLogic<TU> GetSutProperty<TU>(Func<T, TU> sutExtractor, string sutLabel);

        /// <summary>
        /// Specify that the expectation is an instance of some type
        /// </summary>
        /// <param name="expectedType">expected type</param>
        /// <param name="expectedLabel">associated label</param>
        /// <param name="negatedLabel">label when negated</param>
        /// <returns>Continuation object</returns>
        ICheckLogic<T> ExpectingType(System.Type expectedType, string expectedLabel, string negatedLabel);

        /// <summary>
        /// Specify that we expect a list of valies
        /// </summary>
        /// <param name="values">enumeration of values</param>
        /// <param name="count">number of items</param>
        /// <param name="comparison"></param>
        /// <param name="negatedComparison"></param>
        /// <param name="expectedLabel"></param>
        /// <param name="negatedLabel"></param>
        /// <returns>Continuation object</returns>
        ICheckLogic<T> ExpectingValues(IEnumerable values, long count, string comparison = null, string negatedComparison = null, string expectedLabel = null, string negatedLabel = null);

        /// <summary>
        /// Explicitely fails
        /// </summary>
        /// <param name="error">error message</param>
        /// <param name="options">options</param>
        /// <returns>Continuation object</returns>
        ICheckLogic<T> Fails(string error, MessageOption options = MessageOption.None);

        /// <summary>
        /// Set index of interest
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Continuation object</returns>
        ICheckLogic<T> SetValuesIndex(long index);

        /// <summary>
        /// Set values to be given.
        /// </summary>
        /// <typeparam name="TU">Type of reference values</typeparam>
        /// <param name="other"></param>
        /// <param name="comparison"></param>
        /// <param name="negatedComparison"></param>
        /// <returns></returns>
        ICheckLogic<T> ComparingTo<TU>(TU other, string comparison, string negatedComparison);
    }
}