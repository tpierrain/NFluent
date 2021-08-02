// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CheckLogic.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Kernel
{
    using System.Collections;
    using System.Collections.Generic;
    using Extensibility;
    using Extensions;
    using Messages;

// the system namespace is not imported for older Net version. This allows to overload the definition of delegate types. 
#if !DOTNET_35
    using System;
#endif
//    [DebuggerNonUserCode]
    /// <summary>
    /// This class provides a fluent API to implement checks.
    /// It favors ease of use over usual (good) design considerations.
    /// That being said, it can probably be improved.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CheckLogic<T> : ICheckLogic<T>
    {
        // TODO: Refactor
        private const string CantBeUsedWhenNegated = "{0} can't be used when negated";

        private readonly FluentSut<T> fluentSut;
        private ICheckLogicBase child;
        private ICheckLogicBase parent;
        private string comparison;
        private object expected;
        private long expectedCount;
        private ValueKind expectedKind = ValueKind.Value;
        private System.Type expectedType;
        private bool enforceExpectedType;
        private bool failed;
        private bool parentFailed;
        private long? indexInSut;
        private long? indexInExpected;
        private string label;
        private string lastError;

        private string negatedComparison;
        private string negatedError;
        private bool negatedFailed;
        private bool parentNegatedFailed;
        private bool doNotNeedNegatedMessage;
        private bool cannotBetNegated;
        private string negatedLabel;
        private MessageOption negatedOption;
        private MessageOption options = MessageOption.None;

        private bool withExpected;
        private bool withGiven;

        public CheckLogic(FluentSut<T> fluentSut)
        {
            this.fluentSut = fluentSut;
        }

        public bool IsNegated => this.fluentSut.Negated;

        private string LastError => this.IsNegated ? this.negatedError : this.lastError;

        private string Label => this.IsNegated ? this.negatedLabel : this.label;

        private MessageOption Option => this.IsNegated ? this.negatedOption : this.options;

        public EntityNamingLogic SutName => this.fluentSut.SutName;

        public string Comparison => this.IsNegated ? this.negatedComparison : this.comparison;

        public bool Failed => this.failed || this.child != null && this.child.Failed;

        public  ICheckLogic<T> CantBeNegated(string checkName)
        {
            var message = string.Format(CantBeUsedWhenNegated, checkName);
            this.SetNotNegatable(message);
            return this;
        }

        private void SetNotNegatable(string message)
        {
            this.DoNotNeedNegatedMessage();
            this.cannotBetNegated = true;
            if (this.IsNegated)
            {
                throw new System.InvalidOperationException(message);
            }
            this.negatedError = message;
        }

        public ICheckLogic<T> FailIfNull(string error = "The {0} is null.")
        {
            if (this.fluentSut.Value != null)
            {
                return this;
            }

            this.Fail(error, MessageOption.NoCheckedBlock);
            this.DoNotNeedNegatedMessage();
            return this;
        }

        public ICheckLogic<T> DoNotNeedNegatedMessage()
        {
            this.doNotNeedNegatedMessage = true;
            return this;
        }

        public ICheckLogic<T> Analyze(Action<T, ICheckLogic<T>> action)
        {
            if (this.failed || this.parentFailed)
            {
                return this;
            }

            action(this.fluentSut.Value, this);
            return this;
        }

        public ICheckLogic<T> Fail(string error, MessageOption options)
        {
            this.failed = true;
            if (this.IsNegated)
            {
                return this;
            }

            this.lastError = error;
            this.options |= options;
            return this;
        }

        public ICheckLogic<T> SetSutName(string name)
        {
            this.SutName.SetNameBuilder( () => name);
            return this;
        }

        public ICheckLogic<TU> CheckSutAttributes<TU>(Func<T, TU> sutExtractor, string propertyName)
        {
            var sutWrapper = this.fluentSut.Extract(sutExtractor,
                sut => string.IsNullOrEmpty(propertyName) ? sut.SutName.EntityName : $"{sut.SutName.EntityName}'s {propertyName}");
            var result = new CheckLogic<TU>(sutWrapper);

            if (this.cannotBetNegated)
            {
                result.SetNotNegatable(this.negatedError);
            }

            result.parentFailed = this.failed;
            result.parentNegatedFailed = this.negatedFailed;
            result.parent = this;

            this.child = result;
            return result;
        }

        public void EndCheck()
        {
            if (this.parentFailed)
                this.parent.EndCheck();

            if (this.child != null && !this.IsNegated && !this.failed)
            {
                this.child.EndCheck();
            }

            if (this.parent == null && string.IsNullOrEmpty(this.negatedError) && !this.doNotNeedNegatedMessage)
            {
                throw new System.InvalidOperationException("Negated error message was not specified. Use 'OnNegate' method to specify one.");
            }

            if (this.Failed == this.IsNegated)
            {
                return;
            }

            var fluentMessage = FluentMessage.BuildMessage(this.LastError);
            fluentMessage.AddCustomMessage(this.fluentSut.CustomMessage);
            fluentMessage.For(this.SutName);

            if (!this.Option.HasFlag(MessageOption.NoCheckedBlock))
            {
                var block = fluentMessage.On(this.fluentSut.Value, this.indexInSut); 
                block.WithType(this.Option.HasFlag(MessageOption.WithType));
                block.WithHashCode(this.Option.HasFlag(MessageOption.WithHash));

                if (this.fluentSut.Value.IsAnEnumeration(false))
                {
                    block.WithEnumerableCount((this.fluentSut.Value as IEnumerable).Count());
                }
            }

            if ((this.withExpected||this.withGiven) && !this.Option.HasFlag(MessageOption.NoExpectedBlock))
            {
                MessageBlock block;
                switch (this.expectedKind)
                {
                    case ValueKind.Type:
                        block = this.withGiven ? fluentMessage.WithGivenValue(this.expected) : fluentMessage.Expected(this.expected);
                        break;
                    case ValueKind.Values:
                        block = this.withGiven ? fluentMessage.WithGivenValues(this.expected, this.indexInExpected.Value) : fluentMessage.ExpectedValues(this.expected, this.indexInExpected);
                        if (this.expectedCount>0)
                        {
                            block.WithEnumerableCount(this.expectedCount);
                        }
                        break;
                    case ValueKind.Types:
                        block = fluentMessage.Expected(this.expected, this.indexInExpected);
                        break;
                    default:
                        block = (this.IsNegated || this.withGiven)
                            ? fluentMessage.WithGivenValue(this.expected, this.indexInExpected)
                            : fluentMessage.Expected(this.expected, this.indexInExpected);
                        block.WithType(this.Option.HasFlag(MessageOption.WithType));
                        block.WithHashCode(this.Option.HasFlag(MessageOption.WithHash));
                        if (this.expected is IEnumerable list && !(this.expected is string))
                        {
                            block.WithEnumerableCount(this.expected is ICollection collection ? collection.Count: list.Count());
                        }
                        break;
                }

                if (this.expected == null || this.enforceExpectedType)
                {
                    fluentMessage.For(this.expectedType);
                }

                if (!string.IsNullOrEmpty(this.Label))
                {
                    block.Label(this.Label);
                }
                else if (!string.IsNullOrEmpty(this.Comparison))
                {
                    block.Comparison(this.Comparison);
                }
            }


            this.fluentSut.Reporter.ReportError(fluentMessage.ToString());
        }

        public ICheckLogic<T> ComparingTo<TU>(TU givenValue, string comparisonInfo, string negatedComparisonInfo)
        {
            this.DefineExpectations(givenValue, true, comparisonInfo, negatedComparisonInfo);
            return this;
        }

        public ICheckLogic<T> ComparingToValues<TU>(IEnumerable<TU> givenValue, int count, string comparisonInfo, string negatedComparisonInfo)
        {
            this.DefineExpectations(givenValue, true, comparisonInfo, negatedComparisonInfo, count, typeof(TU));
            this.expectedKind = ValueKind.Values;
            return this;
        }

        public ICheckLogic<T> DefineExpectedResult<TU>(TU resultValue, string labelForExpected, string negationForExpected)
        {
            this.DefineExpectations(resultValue, false, labelForExpected, negationForExpected);
            this.label = labelForExpected;
            this.negatedLabel = negationForExpected;
            return this;
        }

        public ICheckLogic<T> DefineExpectedValue<TU>(TU newExpectedValue, string comparisonMessage,
            string negatedComparison1)
        {
            this.DefineExpectations(newExpectedValue, false, comparisonMessage, negatedComparison1);
            return this;
        }

        private void DefineExpectations<TU>(TU newExpectedValue, bool isCompare, string comparisonMessage, string negatedComparison1, long? count =null, System.Type forceType = null)
        {
            if (this.cannotBetNegated && !string.IsNullOrEmpty(negatedComparison1))
            {
                throw new System.InvalidOperationException($"You must not provide a negated comparison label, as {this.negatedError}");
            }
            this.expectedType = forceType ?? (newExpectedValue == null ? typeof(TU) : newExpectedValue.GetType());
            this.expected = newExpectedValue;
            if (isCompare)
            {
                this.withGiven = true;
            }
            else
            {
                this.withExpected = true;
            }
            this.comparison = comparisonMessage;
            this.negatedComparison = negatedComparison1;
            if (count.HasValue)
            {
                this.expectedCount = count.Value;
            }
            else if (newExpectedValue is IEnumerable enumerable)
            {
                this.expectedCount = enumerable.Count();
            }
        }

        public ICheckLogic<T> DefineExpectedType(System.Type expectedInstanceType)
        {
            this.expectedKind = ValueKind.Type;
            this.options |= MessageOption.WithType;
            return this.DefineExpectedValue(new TypeOfInstanceValue(expectedInstanceType), string.Empty, this.doNotNeedNegatedMessage ? "":"different from");
        }

        public ICheckLogic<T> DefineExpectedValues<TU>(IEnumerable<TU> values, long count, string comparisonMessage = null,
            string newNegatedComparison = null)
        {
            this.DefineExpectations(values, false, comparisonMessage, newNegatedComparison, count, typeof(TU));
            this.expectedKind = ValueKind.Values;
            return this;
        }

        public ICheckLogic<T> DefinePossibleTypes(IEnumerable<System.Type> values, string comparisonMessage = null,
            string newNegatedComparison = null)
        {
            this.DefineExpectations(new TypeEnumerationValue(values), false, comparisonMessage, newNegatedComparison,  values.Count(),typeof(System.Type));
            this.expectedKind = ValueKind.Types;
            return this;
        }

        public ICheckLogic<T> DefinePossibleValues<TU>(IEnumerable<TU> values, long count, string comparisonMessage = "one of", string negatedComparison1 = "none of")
        {
            this.DefineExpectations(values, false, comparisonMessage, negatedComparison1, count, typeof(TU));
            this.expectedType = typeof(TU);
            this.enforceExpectedType = true;
            return this;
        }

        public ICheckLogic<T> SetValuesIndex(long indexInSut, long indexInExpected = -1)
        {
            this.indexInSut = indexInSut;
            this.indexInExpected = indexInExpected;
            return this;
        }

        public ICheckLogic<T> OnNegateWhen(Func<T, bool> predicate, string error, MessageOption negatedOptions)
        {
            if (this.negatedFailed || this.parentNegatedFailed)
            {
                return this;
            }

            if (predicate(this.fluentSut.Value))
            {
                this.negatedFailed = true;
                this.negatedError = error;
                this.negatedOption = negatedOptions;
            }

            return this;
        }

        private enum ValueKind
        {
            Value,
            Type,
            Values,
            Types
        }
    }
}