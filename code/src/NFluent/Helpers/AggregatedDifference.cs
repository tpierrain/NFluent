// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AggregatedDifference.cs" company="NFluent">
//   Copyright 2020 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

    internal class AggregatedDifference
    {
        private readonly List<DifferenceDetails> details = new();
        private IList<AggregatedDifference> subs = new List<AggregatedDifference>();
        private bool different;
        public bool IsEquivalent { get; set; }

        public bool IsDifferent => this.different || this.Details.Any();

        public DifferenceDetails this[int id] => this.Details.ElementAt(id);

        private IEnumerable<DifferenceDetails> Details =>this.subs.SelectMany(s => s.Details).Union(this.details);

        public int GetCount(bool forEquivalence = false)
        {
            return this.Details.Count(x=>forEquivalence ? x.StillNeededForEquivalence : x.StillNeededForEquality);
        }

        public void Add(DifferenceDetails detail)
        {
            this.details.Add(detail);
        }

        public void SetAsDifferent(bool state)
        {
            this.different = state;
        }

        public void Merge(AggregatedDifference other)
        {
            this.subs.Add(other);
        }

        public int GetActualIndex()
        {
            return this.Details.FirstOrDefault(x => x.StillNeededForEquality)?.ActualIndex ?? 0;
        }
        
        public int GetExpectedIndex()
        {
            return this.Details.FirstOrDefault(x => x.StillNeededForEquality)?.ActualIndex ?? 0;
        }
        
        public bool DoesProvideDetails(object actual, object expected)
        {
            if (this.Details.Count() == 1)
            {
                return !Equals(this.Details.First().FirstValue, actual);
            }
            return true;
        }

        public string GetErrorMessage(object sut, object expected, bool forEquivalence = false)
        {
            var messageText = new StringBuilder(forEquivalence ? "The {0} is not equivalent to the {1}." : "The {0} is different from the {1}.");
            var errorCount = this.GetCount(forEquivalence);
            if (errorCount > 1)
            {
                messageText.Append($" {errorCount} differences found!");
            }

            if (this.IsEquivalent)
            {
                messageText.Append(" But they are equivalent.");
            }

            if (this.DoesProvideDetails(sut, expected))
            {
                var differenceDetailsCount = Math.Min(ExtensionsCommonHelpers.CountOfLineOfDetails, errorCount);

                if (errorCount - differenceDetailsCount == 1)
                {
                    // we don't want to truncate for one difference
                    differenceDetailsCount++;
                }

                var messages = 0;
                for (var i = 0; messages < differenceDetailsCount; i++)
                {
                    var currentDetails = this.Details.ElementAt(i);
                    if ((forEquivalence && !currentDetails.StillNeededForEquivalence)||(!forEquivalence && !currentDetails.StillNeededForEquality))
                    {
                        continue;
                    }

                    messages++;
                    messageText.AppendLine();
                    messageText.Append(currentDetails.GetMessage(forEquivalence).DoubleCurlyBraces());
                }
                    
                if (differenceDetailsCount != errorCount)
                {
                    messageText.AppendLine();
                    messageText.Append($"... ({errorCount - differenceDetailsCount} differences omitted)");
                }
            }

            return messageText.ToString();
        }
    }
}