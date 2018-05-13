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
    using Extensibility;
    using Helpers;

    /// <summary>
    /// Provides check methods to be executed on a <see cref="EventWaitHandle"/> instance.
    /// </summary>
    public static class EventWaitHandleCheckExtensions
    {
        /// <summary>
        /// Checks that the event is set within a given timeout.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="timeOut">The maximum amount of time before the event should be set (time unit being specified with the timeUnit parameter).</param>
        /// <param name="timeUnit">The time unit of the given timeOut.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The event was not set before the given timeout.</exception>
        public static ICheckLink<ICheck<EventWaitHandle>> IsSetWithin(this ICheck<EventWaitHandle> check, double timeOut, TimeUnit timeUnit)
        {
            var duration = new Duration(timeOut, timeUnit);
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => !sut.WaitOne((int)duration.ToMilliseconds()), 
                    $"The {{checked}} has not been set before the given timeout ({duration}) whereas it must be.", MessageOption.NoCheckedBlock)
                .Negates($"The {{checked}} has been set before the given timeout ({duration}) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the event is not set within a given timeout.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="timeOut">The maximum amount of time before the event should not be set (time unit being specified with the timeUnit parameter).</param>
        /// <param name="timeUnit">The time unit of the given timeOut.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The event was set before the given timeout.</exception>
        public static ICheckLink<ICheck<EventWaitHandle>> IsNotSetWithin(this ICheck<EventWaitHandle> check, double timeOut, TimeUnit timeUnit)
        {
            return check.Not.IsSetWithin(timeOut, timeUnit);
        }
    }
}
