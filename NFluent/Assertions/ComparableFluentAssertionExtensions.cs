// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ComparableFluentAssertionExtensions.cs" company="">
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
namespace NFluent
{
    using System;

    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on an <see cref="IComparable"/> instance.
    /// </summary>
    public static class ComparableFluentAssertionExtensions
    {
        /// <summary>
        /// Determines whether the specified value is before the other one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="otherValue">The other value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The current value is not before the other one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<IComparable>> IsBefore(this IFluentAssertion<IComparable> fluentAssertion, IComparable otherValue)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<IComparable>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<IComparable>;

            return assertionRunner.ExecuteAssertion(() =>
                {
                    if (runnableAssertion.Value == null || runnableAssertion.Value.CompareTo(otherValue) >= 0)
                    {
                        throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis not before:\n\t[{2}]{3}.", runnableAssertion.Value.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(runnableAssertion.Value), otherValue.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(otherValue)));
                    }
                },
                string.Format("\nThe actual value:\n\t[{0}]{1}\nis before:\n\t[{2}]{3}.", runnableAssertion.Value.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(runnableAssertion.Value), otherValue.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(otherValue)));
        }

        private static void IsBeforeImpl(IFluentAssertion<IComparable> fluentAssertion, IComparable otherValue)
        {
            if (fluentAssertion.Value == null || fluentAssertion.Value.CompareTo(otherValue) >= 0)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis not before:\n\t[{2}]{3}.", fluentAssertion.Value.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(fluentAssertion.Value), otherValue.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(otherValue)));
            }
        }

        private static void IsBeforeNegatedImpl(IFluentAssertion<IComparable> fluentAssertion, IComparable otherValue)
        {
            try
            {
                IsBeforeImpl(fluentAssertion, otherValue);
            }
            catch (FluentAssertionException)
            {
                return;
            }

            throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis before:\n\t[{2}]{3}.", fluentAssertion.Value.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(fluentAssertion.Value), otherValue.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(otherValue)));
        }
    }
}