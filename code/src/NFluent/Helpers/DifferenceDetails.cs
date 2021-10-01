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

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Extensions;

    internal class DifferenceDetails
    {
        private readonly DifferenceMode mode;
        private readonly DifferenceDetails[] subs;
        private readonly long expectedIndex;
        private readonly long actualIndex;

        private DifferenceDetails(string firstName, object firstValue, object secondValue, long expectedIndex, long actualIndex, DifferenceMode mode, IEnumerable<DifferenceDetails> subs = null)
        {
            this.mode = mode;
            this.FirstName = firstName;
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.expectedIndex = expectedIndex;
            this.actualIndex = actualIndex;
            if (subs != null)
            {
                this.subs = subs.ToArray();
            }
        }

        public DifferenceDetails this[int id] => this.subs[id];

        public int Count => this.subs?.Length ?? 0;

        public bool IsEquivalent() => this.mode == DifferenceMode.Equivalent;

        public static DifferenceDetails WasNotExpected(string checkedName, object value, long index)
        {
            return new DifferenceDetails(checkedName, value, null, EnumerableExtensions.NullIndex, index, DifferenceMode.Extra);
        }

        public static DifferenceDetails DoesNotHaveExpectedValue(string checkedName, object value, object expected, long sutIndex, long expectedIndex)
        {
            return new DifferenceDetails(checkedName, value, expected, expectedIndex, sutIndex, DifferenceMode.Value);
        }

        public static DifferenceDetails IsDifferent(object value, object expected)
        {
            // Stryker disable once String: Mutation does not alter behaviour
            return new DifferenceDetails(string.Empty, value, expected, EnumerableExtensions.NullIndex, EnumerableExtensions.NullIndex, DifferenceMode.Value);
        }

        public static DifferenceDetails DoesNotHaveExpectedAttribute(string checkedName, object value, object expected, long index = EnumerableExtensions.NullIndex)
        {
            return new DifferenceDetails(checkedName, value, expected, index, index, DifferenceMode.Attribute);
        }

        public static DifferenceDetails DoesNotHaveExpectedDetails(string checkedName, object value, object expected,
            long actualIndex, long expectedIndex, ICollection<DifferenceDetails> details)
        {
            if (details == null || details.Count == 0)
            {
                return null;
            }
            return new DifferenceDetails(checkedName, value, expected, expectedIndex, actualIndex,DifferenceMode.Value, details);
        }

        public static DifferenceDetails DoesNotHaveExpectedDetailsButIsEquivalent(string checkedName, object value,
            object expected,
            long actualIndex, long expectedIndex, ICollection<DifferenceDetails> details)
        {
            return new DifferenceDetails(checkedName, value, expected, expectedIndex, actualIndex, DifferenceMode.Equivalent, details);
        }

        public static DifferenceDetails WasNotFound(string checkedName, object expected, long index)
        {
            return new DifferenceDetails(checkedName, null, expected, index, EnumerableExtensions.NullIndex, DifferenceMode.Missing);
        }

        public static DifferenceDetails WasFoundElseWhere(string checkedName, object value, long expectedIndex, long actualIndex)
        {
            return new DifferenceDetails(checkedName, value, null, expectedIndex, actualIndex, DifferenceMode.Moved);
        }

        public static DifferenceDetails WasFoundInsteadOf(string checkedName, object checkedValue, object expectedValue, long checkedIndex = EnumerableExtensions.NullIndex, long expectedIndex = EnumerableExtensions.NullIndex)
        {
            return new DifferenceDetails(checkedName, checkedValue, expectedValue, expectedIndex, checkedIndex, DifferenceMode.FoundInsteadOf);
        }

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

        public long Index => this.expectedIndex;

        public long ActualIndex => this.actualIndex;

        public void GetFirstDifferenceIndexes(out long actual, out long expected)
        {
            if (this.subs == null || this.subs.Length == 0)
            {
                actual = this.actualIndex;
                expected = this.expectedIndex;
                return;
            }

            var firstDetail = this.subs.FirstOrDefault();

            actual = firstDetail.ActualIndex;
            expected = firstDetail.Index;
        }

        public DifferenceDetails WithoutEquivalenceErrors() => new(this.FirstName, this.FirstValue, this.SecondValue, this.Index, this.ActualIndex, this.mode, this.Details(false));

        private IEnumerable<DifferenceDetails> Details(bool forEquivalence, bool firstLevel = true)
        {
            if (this.subs is {Length: > 0})
            {
                return this.subs.
                    //Where(d => (forEquivalence && (d.StillNeededForEquivalence || d.IsEquivalent()) ) || (!forEquivalence && d.StillNeededForEquality)).
                    SelectMany(s => s.Details(forEquivalence, false));
            }

            if (firstLevel && this.mode == DifferenceMode.Value)
            {
                return Enumerable.Empty<DifferenceDetails>();
            }
            return new[] {this};
        }

        public string GetMessage(bool forEquivalence)
        {
            var messageText = new StringBuilder(forEquivalence ? "The {0} is not equivalent to the {1}." : "The {0} is different from the {1}.");
            var details = this.Details(forEquivalence).ToArray();
            if (details.Length>1)
            {
                messageText.Append($" {details.Length} differences found!");
            }

            if (this.IsEquivalent())
            {
                messageText.Append(" But they are equivalent.");
            }

            if (details.Length == 0)
            {
                return messageText.ToString();
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
                messageText.Append($"... ({details.Length - differenceDetailsCount} differences omitted)");
            }

            return messageText.ToString();
        }

        public string GetDetails(bool forEquivalence)
        {
            return this.mode switch
            {
                DifferenceMode.Extra => forEquivalence
                    ? $"{this.FirstName} value should not exist (value {this.FirstValue.ToStringProperlyFormatted()})"
                    : $"{this.FirstName} should not exist (value {this.FirstValue.ToStringProperlyFormatted()}).",
                DifferenceMode.Missing => forEquivalence
                    ? $"{this.SecondValue.ToStringProperlyFormatted()} should be present but was not found."
                    : $"{this.FirstName} does not exist. Expected {this.SecondValue.ToStringProperlyFormatted()}.",
                DifferenceMode.Moved =>
                $"{this.FirstName} value ('{this.FirstValue}') was found at index {this.actualIndex} instead of {this.Index}.",
                DifferenceMode.Attribute =>
                $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}.",
                DifferenceMode.FoundInsteadOf =>
                $"{this.FirstValue.ToStringProperlyFormatted()} should not exist (found in {this.FirstName}); {this.SecondValue.ToStringProperlyFormatted()} should be found instead.",
                _ =>
                $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}."
            };
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
