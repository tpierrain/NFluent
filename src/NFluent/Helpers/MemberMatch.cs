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

    internal class MemberMatch
    {
        private readonly ReflectionWrapper actual;

        public MemberMatch(ReflectionWrapper expected, ReflectionWrapper actual)
        {
            this.actual = actual;
            this.Expected = expected;
        }

        internal bool DoValuesMatches
        {
            get
            {
                var comparer = new EqualityHelper.EqualityComparer<object>();
                return this.ActualFieldFound && this.ExpectedFieldFound && comparer.Equals(this.actual.Value, this.Expected.Value);
            }
        }

        private ReflectionWrapper Expected { get; }

        /// <summary>
        ///     Gets a actualValue indicating whether the expected field has been found.
        /// </summary>
        internal bool ExpectedFieldFound => this.actual != null;
        internal bool ActualFieldFound => this.Expected != null;

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
                        $"The {{0}}'s {this.Expected.MemberLabel.DoubleCurlyBraces()} has the same value in the comparand, whereas it must not.")
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
                        $"The {{1}}'s {this.Expected.MemberLabel.DoubleCurlyBraces()} is absent from the {{0}}.")
                    .For("value");
                result.Expected(this.Expected.Value).Label($"The {{0}} {this.Expected.MemberLabel.DoubleCurlyBraces()}:");
            }
            else if (!this.ActualFieldFound)
            {
                result = checker.BuildShortMessage(
                        $"The {{0}}'s {this.actual.MemberLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                    .For("value");
                result.On(this.actual.Value).Label($"The {{0}} {this.actual.MemberLabel.DoubleCurlyBraces()}:");
            }
            else
            {
                result = checker.BuildShortMessage(
                        $"The {{0}}'s {this.Expected.MemberLabel.DoubleCurlyBraces()} does not have the expected value.")
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

        public void Check(ICheckLogic<ReflectionWrapper> checkLogic)
        {
            if (this.DoValuesMatches)
            {
                return;
            }

            if (!this.ExpectedFieldFound)
            {
                var expectedLabel = this.Expected.MemberLabel.DoubleCurlyBraces();
                checkLogic
                    .Fails($"The {{1}}'s {expectedLabel} is absent from the {{0}}.", MessageOption.NoCheckedBlock|MessageOption.WithType)
                    .Expecting(this.Expected.Value, expectedLabel, expectedLabel);
            }
            else if (!this.ActualFieldFound)
            {
                checkLogic
                    .GetSutProperty(_ => this.actual.Value, this.actual.MemberLabel)
                    .Fails($"The {{0}}'s {this.actual.MemberLabel.DoubleCurlyBraces()} is absent from the {{1}}.", MessageOption.WithType);
            }
            else
            {
                var expectedLabel = this.Expected.MemberLabel.DoubleCurlyBraces();
                var withType = this.actual.Value.GetTypeWithoutThrowingException() != this.Expected.Value.GetTypeWithoutThrowingException();
                var withHash = !withType && this.actual?.Value != null && this.Expected?.Value != null &&
                               this.actual.Value.ToString() == this.Expected.Value.ToString();
                var mode = (withType ? MessageOption.WithType :
                    MessageOption.None) | (withHash ? MessageOption.WithHash : MessageOption.None);
                checkLogic
                    .GetSutProperty(_=>this.actual.Value, this.actual.MemberLabel)
                    .Fails($"The {{0}}'s {expectedLabel} does not have the expected value.", mode)
                    .Expecting(this.Expected.Value);
                
            }
        }
    }
}