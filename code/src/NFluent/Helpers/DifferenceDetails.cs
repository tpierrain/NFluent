// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DifferenceDetails.cs" company="NFluent">
//   Copyright 2019 Cyrille DUPUYDAUBY
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NFluent.Extensions;

namespace NFluent.Helpers
{

    internal class DifferenceDetails
    {
        private readonly DifferenceMode mode;
        private readonly DifferenceDetails[] subs;

        private DifferenceDetails(string firstName, object firstValue, object secondValue, long expectedIndex, long actualIndex, DifferenceMode mode, IEnumerable<DifferenceDetails> subs = null)
        {
            this.mode = mode;
            this.FirstName = firstName;
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.Index = expectedIndex;
            this.ActualIndex = actualIndex;
            if (subs != null)
            {
                this.subs = subs.ToArray();
            }
        }

        public DifferenceDetails this[int id] => this.subs[id];

        public int Count => this.subs?.Length ?? 0;

        public bool IsEquivalent() => this.mode == DifferenceMode.Equivalent;

        public static DifferenceDetails WasNotExpected(string checkedName, object value, long index) => new DifferenceDetails(checkedName, value, null, EnumerableExtensions.NullIndex, index, DifferenceMode.Extra);

        public static DifferenceDetails DoesNotHaveExpectedValue(string checkedName, object value, object expected, long sutIndex, long expectedIndex) => new DifferenceDetails(checkedName, value, expected, expectedIndex, sutIndex, DifferenceMode.Value);

        public static DifferenceDetails IsDifferent(object value, object expected) =>
            // Stryker disable once String: Mutation does not alter behaviour
            new DifferenceDetails(string.Empty, value, expected, EnumerableExtensions.NullIndex, EnumerableExtensions.NullIndex, DifferenceMode.Value);

        public static DifferenceDetails DoesNotHaveExpectedAttribute(string checkedName, object value, object expected, long index = EnumerableExtensions.NullIndex) => new DifferenceDetails(checkedName, value, expected, index, index, DifferenceMode.Attribute);

        public static DifferenceDetails DoesNotHaveExpectedDetails(string checkedName, object value, object expected,
            long actualIndex, long expectedIndex, ICollection<DifferenceDetails> details) =>
            details.Count == 0 ? null : new DifferenceDetails(checkedName, value, expected, expectedIndex, actualIndex, DifferenceMode.Value, details);

        public static DifferenceDetails DoesNotHaveExpectedDetailsButIsEquivalent(string checkedName, object value,
            object expected, long actualIndex, long expectedIndex, ICollection<DifferenceDetails> details) =>
            new DifferenceDetails(checkedName, value, expected, expectedIndex, actualIndex, DifferenceMode.Equivalent, details);

        public static DifferenceDetails WasNotFound(string checkedName, object expected, long index) 
            => new DifferenceDetails(checkedName, null, expected, index, EnumerableExtensions.NullIndex, DifferenceMode.Missing);

        public static DifferenceDetails WasFoundElseWhere(string checkedName, object value, long expectedIndex, long actualIndex) 
            => new DifferenceDetails(checkedName, value, null, expectedIndex, actualIndex, DifferenceMode.Moved);

        public static DifferenceDetails WasFoundInsteadOf(string checkedName, object checkedValue, object expectedValue, long checkedIndex = EnumerableExtensions.NullIndex, long expectedIndex = EnumerableExtensions.NullIndex) 
            => new DifferenceDetails(checkedName, checkedValue, expectedValue, expectedIndex, checkedIndex, DifferenceMode.FoundInsteadOf);

        public static DifferenceDetails FromMatch(MemberMatch match)
        {
            if (!match.ActualFieldFound)
            {
                return WasNotFound(match.Actual.MemberLabel, match.Actual, 0);
            }

            return match.ExpectedFieldFound ? DoesNotHaveExpectedValue(match.Expected.MemberLabel, match.Actual.Value, match.Expected.Value, 0, 0) 
                : WasNotExpected(match.Expected.MemberLabel, match.Expected, 0);
        }

        public string FirstName { get; internal set; }

        public object FirstValue { get; internal set; }

        public object SecondValue { get; internal set; }

        public long Index { get; }

        public long ActualIndex { get; }

        public void GetFirstDifferenceIndexes(out long actual, out long expected)
        {
            if (this.subs == null || this.subs.Length == 0)
            {
                actual = this.ActualIndex;
                expected = this.Index;
                return;
            }
            // Stryker disable once Linq : Mutation does not alter behaviour
            var firstDetail = this.subs[0];

            actual = firstDetail.ActualIndex;
            expected = subs.Length > 1 ? actual : firstDetail.Index;
        }

        private IEnumerable<DifferenceDetails> Details(bool firstLevel = true)
        {
            if (this.subs != null && this.subs.Length > 0)
            {
                return this.subs.
                    SelectMany(s => s.Details(false));
            }

            if (firstLevel && this.mode == DifferenceMode.Value)
            {
                return Enumerable.Empty<DifferenceDetails>();
            }
            return new[] { this };
        }

        public string GetMessage(bool forEquivalence)
        {
            string messageTemplate = this.FirstValue == null
                ? "The {0} is null whereas it should not."
                : this.SecondValue == null
                    ? "The {0} should be null."
                    : forEquivalence ? "The {0} is not equivalent to the {1}." : "The {0} is different from the {1}.";
            var messageText = new StringBuilder(messageTemplate);

            var details = this.Details().ToArray();
            if (details.Length > 1)
            {
                messageText.AppendFormat(CultureInfo.InvariantCulture, " {0} differences found!", details.Length);
            }

            if (this.IsEquivalent())
            {
                messageText.Append(" But they are equivalent.");
            }

            var differenceDetailsCount = Math.Min(ExtensionsCommonHelpers.CountOfLineOfDetails, details.Length);

            if (details.Length - differenceDetailsCount == 1)
            {
                // we don't truncate the last difference
                differenceDetailsCount++;
            }

            for (var i = 0; i < differenceDetailsCount; i++)
            {
                var currentDetails = details[i];
                messageText.AppendLine();
                messageText.Append(currentDetails.GetDetails(forEquivalence).DoubleCurlyBraces());
            }

            if (differenceDetailsCount != details.Length)
            {
                messageText.AppendLine();
                messageText.AppendFormat(CultureInfo.InvariantCulture, "... ({0} differences omitted)", details.Length - differenceDetailsCount);
            }

            return messageText.ToString();
        }

        public string GetDetails(bool forEquivalence)
        {
            switch (this.mode)
            {
                case DifferenceMode.Extra:
                    return forEquivalence
                        ? $"{this.FirstName} value should not exist (value {this.FirstValue.ToStringProperlyFormatted()})"
                        : $"{this.FirstName} should not exist (value {this.FirstValue.ToStringProperlyFormatted()}).";
                case DifferenceMode.Missing:
                    return forEquivalence
                        ? $"{this.SecondValue.ToStringProperlyFormatted()} should be present but was not found."
                        : $"{this.FirstName} does not exist. Expected {this.SecondValue.ToStringProperlyFormatted()}.";
                case DifferenceMode.Moved:
                    return $"{this.FirstName} value ('{this.FirstValue}') was found at index {this.ActualIndex} instead of {this.Index}.";
                case DifferenceMode.Attribute:
                    return $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}.";
                case DifferenceMode.FoundInsteadOf:
                    return
                        $"{this.FirstValue.ToStringProperlyFormatted()} should not exist (found in {this.FirstName}); {this.SecondValue.ToStringProperlyFormatted()} should be found instead.";
                default:
                    return $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}.";
            }
        }

        public enum DifferenceMode
        {
            Attribute,
            Value,
            Equivalent,
            Missing,
            Extra,
            Moved,
            FoundInsteadOf
        }
    }
}
