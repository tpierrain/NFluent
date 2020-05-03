 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="IChecker.cs" company="">
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
    using Kernel;
#if !DOTNET_35
    using System;
#endif

    /// <summary>
    /// Provides a mean to execute some checks on a value, taking care of whether it should be negated or not, etc.
    /// This interface is designed for developers that need to add new check (extension) methods.
    /// Thus, it should not be exposed via Intellisense to developers that are using NFluent to write 
    /// checks statements.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the value to assert on.
    /// </typeparam>
    /// <typeparam name="TC">Interface for the check.
    /// </typeparam>
    public interface IChecker<out T, out TC> : INegated, IWithValue<T> where TC : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        /// <summary>
        /// Sets an optional label for the SUT to be used instead of the default one for message generation.
        /// </summary>
        /// <param name="sutLabel">The label for the SUT.</param>
        void SetSutLabel(string sutLabel);

        /// <summary>
        /// Builds an helper object to ease coding of checks.
        /// </summary>
        /// <returns>An instance of <see cref="ICheckLogic{T}"/>.</returns>
        ICheckLogic<T> BeginCheck();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TU"></typeparam>
        /// <param name="extractor"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        ICheck<TU> ExtractSub<TU>(Func<T, TU> extractor, string label);

        /// <summary>
        /// Gets the check link to return for the next check to be executed (linked with the And operator).
        /// This property is only useful for those that doesn't want to implement their check methods with the 
        /// <see cref="ExecuteCheck"/> method.
        /// </summary>
        /// <returns>
        ///     The check link to return for next check (linked with the And operator) to be executed.
        /// </returns>
        ICheckLink<TC> BuildChainingObject();

        /// <summary>
        /// Executes the check provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">
        ///     The happy-path action (vs. the one for negated version which has not to be specified). 
        ///     This lambda should simply return if everything is ok, or throws a 
        ///     <see cref="FluentCheckException"/> otherwise.</param>
        /// <param name="negatedExceptionMessage">
        ///     The message for the exception to be thrown when the check fails, in the case we were running the negated version.
        /// </param>
        /// <returns>
        ///     The <see cref="BuildChainingObject"/>.
        /// </returns>
        /// <exception cref="FluentCheckException">The check fails.</exception>
        ICheckLink<TC> ExecuteCheck(
#if DOTNET_35
            System.
#endif
                Action action, string negatedExceptionMessage);

        /// <summary>
        /// Executes the check provided as an happy-path lambda (vs lambda for negated version) and returns a not linkable check.
        /// </summary>
        /// <param name="action">The happy-path action (vs. the one for negated version which has not to be specified).
        /// This lambda should simply return if everything is ok, or throws a
        /// <see cref="FluentCheckException" /> otherwise.</param>
        /// <param name="negatedExceptionMessage">The message for the exception to be thrown when the check fails, in the case we were running the negated version.</param>
        /// <exception cref="FluentCheckException">The check fails.</exception>
        void ExecuteNotChainableCheck(
#if DOTNET_35
            System.
#endif
            Action action, string negatedExceptionMessage);

        /// <summary>
        /// Builds an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="FluentMessage"/> instance.</returns>
        FluentMessage BuildMessage(string message);

        /// <summary>
        /// Builds an error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="FluentMessage"/> instance.</returns>
        FluentMessage BuildShortMessage(string message);
    }
}