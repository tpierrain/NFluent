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
        internal readonly ReflectionWrapper Actual;

        public MemberMatch(ReflectionWrapper expected, ReflectionWrapper actual)
        {
            this.Actual = actual;
            this.Expected = expected;
        }

        internal bool DoValuesMatches
        {
            get
            {
                var comparer = new EqualityHelper.EqualityComparer<object>();
                return this.ActualFieldFound && this.ExpectedFieldFound && comparer.Equals(this.Actual.Value, this.Expected.Value);
            }
        }

        internal ReflectionWrapper Expected { get; }

        /// <summary>
        ///     Gets a actualValue indicating whether the expected field has been found.
        /// </summary>
        internal bool ExpectedFieldFound => this.Actual != null;
        internal bool ActualFieldFound => this.Expected != null;

        public void Check(ICheckLogic<ReflectionWrapper> checkLogic)
        {
            if (!this.ExpectedFieldFound)
            {
                checkLogic
                    .CheckSutAttributes(_ => this.Expected.Value, this.Expected.MemberLabel)
                    .Fail("The {1}'s is absent from the {0}.", MessageOption.NoCheckedBlock|MessageOption.WithType)
                    .DefineExpected(this.Expected.Value);
            }
            else if (!this.ActualFieldFound)
            {
                checkLogic
                    .CheckSutAttributes(_ => this.Actual.Value, this.Actual.MemberLabel)
                    .Fail("The {0} is absent from the {1}.", MessageOption.WithType);
            }
            else
            {
                var withType = this.Actual.Value.GetTypeWithoutThrowingException() != this.Expected.Value.GetTypeWithoutThrowingException();
                var withHash = !withType &&
                               this.Actual.Value.ToStringProperlyFormatted() == this.Expected.Value.ToStringProperlyFormatted();
                var mode = (withType ? MessageOption.WithType :
                    MessageOption.None) | (withHash ? MessageOption.WithHash : MessageOption.None);
                if (this.DoValuesMatches)
                {
                    checkLogic
                        .CheckSutAttributes(_=>this.Actual.Value, this.Actual.MemberLabel)
                        .Fail("The {0} has the same value than the given one, whereas it should not.", MessageOption.NoCheckedBlock)
                        .ComparingTo(this.Expected.Value, "different from", "");
                }
                else
                {
                    checkLogic
                        .CheckSutAttributes(_=>this.Actual.Value, this.Actual.MemberLabel)
                        .Fail("The {0} does not have the expected value.", mode)
                        .DefineExpected(this.Expected.Value);
                }
            }
        }
    }
}