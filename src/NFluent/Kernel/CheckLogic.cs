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
    using Helpers;

#if !DOTNET_35 && !DOTNET_20 && !DOTNET_30
    using System;
#endif

    internal class CheckLogic<T> : ICheckLogic<T>
    {
        private enum ValueKind
        {
            Value,
            Type,
            Values
        }

        private readonly T value;
        private readonly bool inverted;
        private string lastError;
        private bool failed;

        private bool withExpected;
        private object expected;
        private string label;
        private string negatedLabel;
        private string comparison;
        private MessageOption options = MessageOption.None;
        
        private string negatedComparison;
        private bool negatedFailed;
        private string negatedError;
        private MessageOption negatedOption;
        private string sutName;
        private string checkedLabel;
        private ValueKind expectedKind = ValueKind.Value;
        private long expectedCount;
        private long index;

        public CheckLogic(T value, string label, bool inverted)
        {
            this.value = value;
            this.inverted = inverted;
            this.sutName = label;
        }

        private bool IsNegated => this.inverted;

        public string LastError => (this.IsNegated ? this.negatedError : this.lastError);

        public string Label => (this.IsNegated ? this.negatedLabel : this.label);

        public MessageOption Option => (this.IsNegated ? this.negatedOption : this.options);

        public string Comparison => this.IsNegated ? this.negatedComparison: this.comparison;

        public ICheckLogic<T> FailsIf(Func<T, bool> predicate, string error, MessageOption noCheckedBlock)
        {
            if (this.failed)
            {
                return this;
            }
            this.failed =  predicate(this.value);
            if (this.failed && !this.IsNegated)
            {
                this.lastError = error;
                this.options = this.options | noCheckedBlock;
            }
            return this;
        }

        public ICheckLogic<T> Fails(string error, MessageOption noCheckedBlock)
        {
            this.failed =  true;
            if (this.failed && !this.IsNegated)
            {
                this.lastError = error;
                this.options = this.options | noCheckedBlock;
            }

            return this;
        }

        public ICheckLogic<T> FailsIfNull(string error)
        {
            return this.FailsIf((sut) => sut == null, error, MessageOption.NoCheckedBlock | MessageOption.ForceType);
        }

        public ICheckLogic<T> SutNameIs(string name)
        {
            this.sutName = name;
            return this;
        }

        // TODO: improve sut naming logic on extraction
        public ICheckLogic<TU> GetSutProperty<TU>(Func<T, TU> sutExtractor, string newSutLabel)
        {
            var result = new CheckLogic<TU>(this.value == null ? default(TU) : sutExtractor(this.value), null, this.inverted) {checkedLabel = newSutLabel};
            return result;
        }

        public void EndCheck()
        {
            if (this.failed == this.IsNegated)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.LastError))
            {
                throw new System.InvalidOperationException("Error message was not specified.");
            }

            var fluentMessage = FluentMessage.BuildMessage(this.LastError);
            if ((this.Option & MessageOption.NoCheckedBlock) == 0)
            {
                var block = fluentMessage.On(this.value, this.index);
                if (!string.IsNullOrEmpty(this.checkedLabel))
                {
                    block.Label(this.checkedLabel);
                }

                if ((this.expectedKind == ValueKind.Values) && (this.value != null))
                {
                    block.WithEnumerableCount(((IEnumerable) this.value).Count());
                }
            }
            
            if (!PolyFill.IsNullOrWhiteSpace(this.sutName))
            {
                fluentMessage.For(this.sutName);
            }
            else
            {
                fluentMessage.For(typeof(T));
            }
            
            if (this.withExpected && (this.Option & MessageOption.NoExpectedBlock) == MessageOption.None)
            {
                MessageBlock block;
                switch (this.expectedKind)
                {
                    case ValueKind.Type:
                        block = fluentMessage.ExpectedType((System.Type) this.expected);
                        break;
                    case ValueKind.Value:
                    default:
                        block = fluentMessage.Expected(this.expected);
                        break;
                    case ValueKind.Values:
                        block = fluentMessage.ExpectedValues(this.expected, this.index).WithEnumerableCount(this.expectedCount);
                        break;
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

            if ((this.options & MessageOption.ForceType) == MessageOption.ForceType)
            {
                fluentMessage.For(typeof(T));
            }

            throw ExceptionHelper.BuildException(fluentMessage.ToString());
        }

        public ICheckLogic<T> Expecting<TU>(TU newExpectedValue, string comparisonMessage = null,
            string negatedComparison1 = null, string expectedLabel = null, string negatedExpLabel = null)
        {
            this.comparison = comparisonMessage;
            this.negatedComparison = negatedComparison1;
            this.withExpected = true;
            this.expected = newExpectedValue;
            this.label = expectedLabel;
            this.negatedLabel = negatedExpLabel ?? expectedLabel;
            return this;
        }

        public ICheckLogic<T> ExpectingType(System.Type expectedType, string expectedLabel, string negatedExpLabel)
        {
            this.expectedKind = ValueKind.Type;
            return this.Expecting(expectedType, expectedLabel: expectedLabel, negatedExpLabel: negatedExpLabel);
        }

        public ICheckLogic<T> ExpectingValues(IEnumerable values, long count)
        {
            this.expectedKind = ValueKind.Values;
            this.expectedCount = count;
            return this.Expecting(values);
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

            if (predicate(this.value))
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
            action(this.value, this);
            return this;
        }
    }
}