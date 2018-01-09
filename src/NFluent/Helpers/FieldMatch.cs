// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FieldMatch.cs" company="">
// //   Copyright 2018 Cyrille DUPUYDAUBY
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

namespace NFluent.Helpers
{
    using Extensibility;
    using Extensions;

    internal class FieldMatch
    {
        private readonly ExtendedFieldInfo actual;

        public FieldMatch(ExtendedFieldInfo expected, ExtendedFieldInfo actual)
        {
            this.actual = actual;
            this.Expected = expected;
        }

        private bool DoValuesMatches
        {
            get
            {
                var comparer = new EqualityHelper.EqualityComparer<object>();
                return this.ExpectedFieldFound && comparer.Equals(this.actual.Value, this.Expected.Value);
            }
        }

        private ExtendedFieldInfo Expected { get; }

        /// <summary>
        ///     Gets a actualValue indicating whether the expected field has been found.
        /// </summary>
        private bool ExpectedFieldFound => this.actual != null;

        public FluentMessage BuildMessage<T>(IChecker<T, ICheck<T>> checker, bool negated)
        {
            FluentMessage result;
            if (this.DoValuesMatches != negated)
            {
                return null;
            }

            if (negated)
            {
                result = checker.BuildShortMessage(
                        $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} has the same value in the comparand, whereas it must not.")
                    .For("value");
                EqualityHelper.FillEqualityErrorMessage(
                    result,
                    this.actual.Value,
                    this.Expected.Value,
                    true,
                    false);
            }
            else if (!this.ExpectedFieldFound)
            {
                result = checker.BuildShortMessage(
                        $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                    .For("value");
                result.Expected(this.Expected.Value);
            }
            else
            {
                result = checker.BuildShortMessage(
                        $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} does not have the expected value.")
                    .For("value");
                EqualityHelper.FillEqualityErrorMessage(
                    result,
                    this.actual.Value,
                    this.Expected.Value,
                    false,
                    false);
            }

            return result;
        }
    }
}