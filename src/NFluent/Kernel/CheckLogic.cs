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
    using Extensibility;
    using Extensions;
    using Helpers;
#if !DOTNET_35 && !DOTNET_20 && !DOTNET_30
    using System;
#endif

    internal class CheckLogic<T> : ICheckLogic<T>
    {
        private readonly FluentSut<T> fluentSut;
        private ICheckLogicBase child;
        private bool isRoot;
        private string comparison;
        private object expected;
        private long expectedCount;
        private ValueKind expectedKind = ValueKind.Value;
        private System.Type expectedType;
        private bool failed;
        private long index;
        private string label;
        private string lastError;

        private string negatedComparison;
        private string negatedError;
        private bool negatedFailed;
        private string negatedLabel;
        private MessageOption negatedOption;
        private MessageOption options = MessageOption.None;
        private string sutName;

        private bool withExpected;
        private bool withGiven;

        public CheckLogic(FluentSut<T> fluentSut)
        {
            this.fluentSut = fluentSut;
            this.isRoot = true;
        }

        private bool IsNegated { get => this.fluentSut.Negated; }

        public string LastError => this.IsNegated ? this.negatedError : this.lastError;

        public string Label => this.IsNegated ? this.negatedLabel : this.label;

        public MessageOption Option => this.IsNegated ? this.negatedOption : this.options;

        public string SutName => string.IsNullOrEmpty(this.fluentSut.SutName) ? this.sutName : this.fluentSut.SutName;

        public string Comparison => this.IsNegated ? this.negatedComparison : this.comparison;

        public bool Failed => this.failed || this.child != null && this.child.Failed;

        public ICheckLogic<T> FailsIf(Func<T, bool> predicate, string error, MessageOption options)
        {
            return this.FailsIf(predicate, (x, y) => error, options);
        }

        public ICheckLogic<T> FailsIf(Func<T, bool> predicate, Func<T, ICheckLogic<T>, string> errorBuilder,
            MessageOption noCheckedBlock)
        {
            if (this.failed)
            {
                return this;
            }

            this.failed = predicate(this.fluentSut.Value);
            if (!this.failed || this.IsNegated)
            {
                return this;
            }

            this.lastError = errorBuilder(this.fluentSut.Value, this);
            this.options = this.options | noCheckedBlock;
            return this;
        }

        public ICheckLogic<T> Fails(string error, MessageOption noCheckedBlock)
        {
            this.failed = true;
            if (!this.IsNegated)
            {
                this.lastError = error;
                this.options = this.options | noCheckedBlock;
            }

            return this;
        }

        public ICheckLogic<T> FailsIfNull(string error)
        {
            return this.FailsIf(sut => sut == null, error, MessageOption.NoCheckedBlock);
        }

        public ICheckLogic<T> InvalidIf(Func<T, bool> predicate, string error)
        {
            if (predicate(this.fluentSut.Value))
            {
                throw new System.InvalidOperationException(error);
            }
            return this;
        }

        public ICheckLogic<T> SutNameIs(string name)
        {
            this.sutName = name;
            return this;
        }

        // TODO: improve sut naming logic on extraction
        public ICheckLogic<TU> GetSutProperty<TU>(Func<T, TU> sutExtractor, string newSutLabel)
        {
            var value = this.fluentSut.Value;
            var result =
                new CheckLogic<TU>(new FluentSut<TU>(value == null ? default(TU) : sutExtractor(value),
                    this.IsNegated)) {isRoot = false};

            var sutname = string.IsNullOrEmpty(this.sutName) ? (this.fluentSut.SutName??"value") : this.sutName;
            if (!string.IsNullOrEmpty(newSutLabel))
            {
                result.SutNameIs($"{sutname}'s {newSutLabel}");
            }
            if (this.failed != this.IsNegated)
            {
                result.failed = this.failed;
                result.negatedFailed = this.negatedFailed;
                result.lastError = this.lastError;
                result.negatedError = this.negatedError;
                result.negatedOption = this.negatedOption;
                result.options = this.options;
            }

            this.child = result;
            return result;
        }

        public bool EndCheck()
        {
            this.child?.EndCheck();

            if (this.Failed == this.IsNegated)
            {
                return true;
            }

            if (string.IsNullOrEmpty(this.LastError))
            {
                if (!this.isRoot)
                {
                    return false;
                }
                throw new System.InvalidOperationException("Error message was not specified.");
            }

            var fluentMessage = FluentMessage.BuildMessage(this.LastError);
            if ((this.Option & MessageOption.NoCheckedBlock) == 0)
            {
                var block = fluentMessage.On(this.fluentSut.Value, this.index);

                if (this.Option.HasFlag(MessageOption.WithType))
                {
                    block.WithType();
                }

                if (this.fluentSut.Value == null || typeof(T).IsNullable())
                {
                    block.OfType(typeof(T));
                    fluentMessage.For(typeof(T));
                }

                block.WithHashCode(this.Option.HasFlag(MessageOption.WithHash));

                if (this.fluentSut.Value is IEnumerable list && !(this.fluentSut.Value is string))
                {
                    block.WithEnumerableCount(list.Count());
                }
            }
            else
            {
                fluentMessage.For(typeof(T));
            }

            if (this.withExpected && (this.Option & MessageOption.NoExpectedBlock) == MessageOption.None)
            {
                MessageBlock block;
                if (this.expectedKind == ValueKind.Type)
                {
                    block = fluentMessage.ExpectedType((System.Type) this.expected);
                }
                else if (this.expectedKind == ValueKind.Values)
                {
                    block = fluentMessage.ExpectedValues(this.expected, this.index)
                        .WithEnumerableCount(this.expectedCount);
                }
                else
                {
                    block = fluentMessage.Expected(this.expected);
                    block.WithType(this.Option.HasFlag(MessageOption.WithType));
                    block.WithHashCode(this.Option.HasFlag(MessageOption.WithHash));
                }

                if (this.expected == null)
                {
                    block.OfType(this.expectedType);
                    fluentMessage.For(this.expectedType);
                }

                if (!string.IsNullOrEmpty(this.Comparison))
                {
                    block.Comparison(this.Comparison);
                }
                else
                {
                    block.Label(this.Label);
                }
            }
            else if (this.withGiven)
            {
                fluentMessage.WithGivenValue(this.expected).Comparison(this.Comparison);
            }

            if (!PolyFill.IsNullOrWhiteSpace(this.SutName))
            {
                fluentMessage.For(this.SutName);
            }

            throw ExceptionHelper.BuildException(fluentMessage.ToString());
        }

        public ICheckLogic<T> ComparingTo<TU>(TU givenValue, string comparison, string negatedComparison)
        {
            this.comparison = comparison;
            this.negatedComparison = negatedComparison;
            this.expected = givenValue;
            this.withGiven = true;
            return this;
        }

        public ICheckLogic<T> Expecting<TU>(TU newExpectedValue, string comparisonMessage = null,
            string negatedComparison1 = null, string expectedLabel = null, string negatedExpLabel = null)
        {
            this.expectedType = newExpectedValue == null ? typeof(TU) : newExpectedValue.GetType();
            this.withExpected = true;
            this.expected = newExpectedValue;
            this.comparison = comparisonMessage;
            this.negatedComparison = negatedComparison1;
            this.label = expectedLabel;
            this.negatedLabel = negatedExpLabel ?? expectedLabel;
            return this;
        }

        public ICheckLogic<T> ExpectingType(System.Type expectedType, string expectedLabel, string negatedExpLabel)
        {
            this.expectedKind = ValueKind.Type;
            this.options |= MessageOption.WithType;
            return this.Expecting(expectedType, expectedLabel, negatedExpLabel);
        }

        public ICheckLogic<T> ExpectingValues(IEnumerable values, long count, string comparisonMessage = null,
            string negatedComparison = null, string expectedLabel = null, string negatedExpLabel = null)
        {
            this.expectedKind = ValueKind.Values;
            this.expectedCount = count;
            return this.Expecting(values, comparisonMessage, negatedComparison, expectedLabel, negatedExpLabel);
        }

        public ICheckLogic<T> SetValuesIndex(long index)
        {
            this.index = index;
            return this;
        }

        public ICheckLogic<T> Negates(string message, MessageOption option)
        {
            if (this.negatedFailed)
            {
                return this;
            }

            this.negatedError = message;
            this.negatedFailed = true;
            this.negatedOption = option;
            return this;
        }

        public ICheckLogic<T> NegatesIf(Func<T, bool> predicate, string error, MessageOption option)
        {
            if (this.negatedFailed)
            {
                return this;
            }

            if (predicate(this.fluentSut.Value))
            {
                this.negatedFailed = true;
                this.negatedError = error;
                this.negatedOption = option;
            }

            return this;
        }

        public ICheckLogic<T> Analyze(Action<T, ICheckLogic<T>> action)
        {
            if (this.failed)
            {
                return this;
            }

            action(this.fluentSut.Value, this);
            return this;
        }

        private enum ValueKind
        {
            Value,
            Type,
            Values
        }
    }
}