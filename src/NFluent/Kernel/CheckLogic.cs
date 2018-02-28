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
    using Extensibility;
    using Helpers;

#if !DOTNET_35
    using System;
#endif

    internal class CheckLogic<T, TC> : ICheckLogic<T>
        where TC : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        private readonly IChecker<T, TC> checker;
        private readonly bool inverted;
        private string lastError;
        private bool failed;

        private bool withExpected;
        private object expected;
        private string label;
        private string comparison;
        private string negatedComparison;
        private MessageOption options = MessageOption.None;
        
        private bool negatedFailed;
        private string negatedError;

        public CheckLogic(IChecker<T, TC> checker, bool inverted)
        {
            this.checker = checker;
            this.inverted = inverted;
        }

        private bool IsNegated => this.inverted != this.checker.Negated;

        public string LastError => this.IsNegated ? this.negatedError : this.lastError;

        public string Comparison => this.IsNegated ? this.negatedComparison: this.comparison;

        public ICheckLogic<T> FailsIf(Func<T, bool> predicate, string error, MessageOption noCheckedBlock)

        {
            if (this.failed)
            {
                return this;
            }
            this.failed =  predicate(this.checker.Value);
            if (this.failed && !this.IsNegated)
            {
                this.lastError = error;
                this.options = this.options | noCheckedBlock;
            }
            return this;
        }

        public void EndCheck()
        {
            if (this.failed == this.IsNegated)
            {
                return;
            }

            var fluentMessage = (this.options & MessageOption.NoCheckedBlock) == MessageOption.NoCheckedBlock ? 
                this.checker.BuildShortMessage(this.LastError) : 
                this.checker.BuildMessage(this.LastError);

            if (this.withExpected && (this.options & MessageOption.NoExpectedBlock) == MessageOption.None)
            {
                var block = fluentMessage.Expected(this.expected);
                if (!string.IsNullOrEmpty(this.Comparison))
                {
                    block.Comparison(this.Comparison);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.label))
                    {
                        block.Label(this.label);
                    }
                }
            }

            throw ExceptionHelper.BuildException(fluentMessage.ToString());
        }

        public ICheckLogic<T> Expecting<TU>(TU newExpectedValue, string comparisonMessage = null,
            string negatedComparison1 = null, string expectedLabel = null)
        {
            this.comparison = comparisonMessage;
            this.negatedComparison = negatedComparison1;
            this.withExpected = true;
            this.expected = newExpectedValue;
            this.label = expectedLabel;
            return this;
        }

        public ICheckLogic<T> Negates(string message)
        {
            this.negatedError = message;
            this.negatedFailed = true;
            return this;
        }

        public ICheckLogic<T> NegatesIf(Func<T, bool> predicate, string error)
        {
            if (this.negatedFailed)
            {
                return this;
            }

            if (predicate(this.checker.Value))
            {
                this.negatedFailed = true;
                this.negatedError = error;
            }

            return this;
        }
    }
}