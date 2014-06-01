// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EventWaitHandleCheckExtensions.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    using System.Threading;
    using NFluent.Extensibility;

    /// <summary>
    /// Provides check methods to be executed on a <see cref="EventWaitHandle"/> instance.
    /// </summary>
    public static class EventWaitHandleCheckExtensions
    {
        /// <summary>
        /// Checks that the event is set before a given timeout in millisecond.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="timeOutInMsec">The maximum amount of milliseconds before the event should be set.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The event was not set before the given timeout in millisecond.
        /// </exception>
        public static ICheckLink<ICheck<EventWaitHandle>> IsSetBefore(this ICheck<EventWaitHandle> check, int timeOutInMsec)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            
            return checker.ExecuteCheck(
                () =>
                {
                    if (!checker.Value.WaitOne(timeOutInMsec))
                    {
                        var errorMessage = FluentMessage.BuildMessage(string.Format("The checked event has not been set before the given timeout.\nThe given timeout (in msec):\n\t[{0}]", timeOutInMsec)).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage(string.Format("The checked event has been set before the given timeout whereas it must not.\nThe given timeout (in msec):\n\t[{0}]", timeOutInMsec)).ToString());
        }

        /// <summary>
        /// Checks that the event is not set before a given timeout in millisecond.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="timeOutInMsec">The maximum amount of milliseconds before the event should not be set.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The event was set before the given timeout in millisecond.
        /// </exception>
        public static ICheckLink<ICheck<EventWaitHandle>> IsNotSetBefore(this ICheck<EventWaitHandle> check, int timeOutInMsec)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (checker.Value.WaitOne(timeOutInMsec))
                    {
                        var errorMessage = FluentMessage.BuildMessage(string.Format("The checked event has been set before the given timeout.\nThe given timeout (in msec):\n\t[{0}]", timeOutInMsec)).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage(string.Format("The checked event has not been set before the given timeout whereas it must.\nThe given timeout (in msec):\n\t[{0}]", timeOutInMsec)).ToString());
        }
    }
}
