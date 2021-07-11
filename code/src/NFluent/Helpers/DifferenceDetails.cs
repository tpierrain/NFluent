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

        private DifferenceDetails(string firstName, object firstValue, object secondValue, int index, DifferenceMode mode, IEnumerable<DifferenceDetails> subs = null)
        {
            this.mode = mode;
            this.FirstName = firstName;
            this.FirstValue = firstValue;
            this.SecondValue = secondValue;
            this.Index = index;
            this.ActualIndex = index;
            if (subs != null)
            {
                this.subs = subs.ToArray();
            }
        }

        public DifferenceDetails this[int index] => this.subs[index];

        public int Count => this.subs?.Length ?? 0;

        public bool StillNeededForEquivalence => this.mode != DifferenceMode.Moved && this.mode !=DifferenceMode.Value;

        public bool StillNeededForEquality => this.mode != DifferenceMode.FoundInsteadOf;

        public bool IsEquivalent()
        {
            return this.mode == DifferenceMode.Equivalent;
        }

        public static DifferenceDetails WasNotExpected(string checkedName, object value, int index)
        {
            return new(checkedName, value, null, index, DifferenceMode.Extra);
        }

        public static DifferenceDetails DoesNotHaveExpectedValue(string checkedName, object value, object expected, int index)
        {
            return new(checkedName, value, expected, index, DifferenceMode.Value);
        }

        public static DifferenceDetails EntryDoesNotHaveExpectedValue(string checkedName, object value, object expected, int actualIndex, int expectedIndex)
        {
            return new(checkedName, value, expected, expectedIndex, DifferenceMode.Value) { ActualIndex = actualIndex};
        }

        public static DifferenceDetails DoesNotHaveExpectedAttribute(string checkedName, object value, object expected, int index = -1)
        {
            return new(checkedName, value, expected, index, DifferenceMode.Attribute);
        }

        public static DifferenceDetails DoesNotHaveExpectedDetails(string checkedName, object value, object expected,
            int actualIndex, int expectedIndex, ICollection<DifferenceDetails> details)
        {
            if (details == null || details.Count == 0)
            {
                return null;
            }
            return new(checkedName, value, expected, expectedIndex, DifferenceMode.Value, details) { ActualIndex = actualIndex};
        }

        public static DifferenceDetails DoesNotHaveExpectedDetailsButIsEquivalent(string checkedName, object value,
            object expected,
            int actualIndex, int expectedIndex, ICollection<DifferenceDetails> details)
        {
            return new(checkedName, value, expected, expectedIndex, DifferenceMode.Equivalent, details) { ActualIndex = actualIndex};
        }

        public static DifferenceDetails WasNotFound(string checkedName, object expected, int index)
        {
            return new(checkedName, null, expected, index, DifferenceMode.Missing);
        }

        public static DifferenceDetails WasFoundElseWhere(string checkedName, object value, int expectedIndex, int actualIndex)
        {
            return new(checkedName, value, null, expectedIndex, DifferenceMode.Moved) { ActualIndex = actualIndex };
        }

        public static DifferenceDetails WasFoundInsteadOf(string checkedName, object checkedValue, object expectedValue)
        {
            return new(checkedName, checkedValue, expectedValue, 0, DifferenceMode.FoundInsteadOf);
        }

        public static DifferenceDetails FromMatch(MemberMatch match)
        {
            if (!match.ActualFieldFound)
            {
                return WasNotFound(match.Actual.MemberLabel, match.Actual, 0);
            }

            return match.ExpectedFieldFound ? DoesNotHaveExpectedValue(match.Expected.MemberLabel, match.Actual.Value, match.Expected.Value, 0) : WasNotExpected(match.Expected.MemberLabel, match.Expected, 0);
        }
        
        public string FirstName { get; internal set; }

        public object FirstValue { get; internal set; }

        public object SecondValue { get; internal set; }

        public int Index { get; }

        public int ActualIndex { get; internal set; }

        private IEnumerable<DifferenceDetails> Details(bool forEquivalence)
        {
            if (this.subs != null && this.subs.Length > 0)
            {
                return this.subs.Where(d => (forEquivalence && d.StillNeededForEquivalence) || (!forEquivalence && d.StillNeededForEquality)).SelectMany(s => s.Details(forEquivalence));
            }

            if (this.mode == DifferenceMode.Value)
            {
                return Enumerable.Empty<DifferenceDetails>();
            }

            return new[] {this};
        }

        public string GetMessage(bool forEquivalence)
        {
            var messageText = new StringBuilder(forEquivalence ? "The {0} is not equivalent to the {1}." : "The {0} is different from the {1}.");
            var details = Details(forEquivalence).ToArray();
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

            foreach (var currentDetails in details)
            {
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
            if (this.subs is {Length: > 0})
            {
                return this.subs[0].GetDetails(forEquivalence);
            }
            return this.mode switch
            {
                DifferenceMode.Extra => forEquivalence
                    ? $"{this.FirstName} value should not exist (value {this.FirstValue.ToStringProperlyFormatted()})"
                    : $"{this.FirstName} should not exist (value {this.FirstValue.ToStringProperlyFormatted()}).",
                DifferenceMode.Missing => forEquivalence
                    ? $"{this.SecondValue.ToStringProperlyFormatted()} should be present but was not found."
                    : $"{this.FirstName} does not exist. Expected {this.SecondValue.ToStringProperlyFormatted()}.",
                DifferenceMode.Moved =>
                $"{this.FirstName} value ('{this.FirstValue}') was found at index {this.ActualIndex} instead of {this.Index}.",
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
