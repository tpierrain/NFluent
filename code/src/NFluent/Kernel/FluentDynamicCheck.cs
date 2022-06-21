// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentDynamicCheck.cs" company="">
//   Copyright 2017 Cyrille DUPUYDAUBY
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

namespace NFluent
{
#if !DOTNET_35
    using Extensibility;
    using Kernel;
    using Messages;

    /// <summary>
    ///     Provides fluent check methods to be executed on a given value.
    /// </summary>
    public class FluentDynamicCheck : IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense,
        IForkableCheck
    {
        private readonly dynamic value;
        private readonly IErrorReporter reporter;
        private bool negated;

        /// <summary>
        /// Builds an instance of <see cref="FluentDynamicCheck"/>
        /// </summary>
        /// <param name="value">dynamic value to check</param>
        /// <param name="reporter">failure reporter</param>
        public FluentDynamicCheck(dynamic value, IErrorReporter reporter)
        {
            this.value = value;
            this.reporter = reporter;
        }

        /// <summary>
        /// Checks if the given dynamic is null.
        /// </summary>
        public DynamicCheckLink IsNotNull()
        {
            if (this.negated != (this.value != null))
            {
                return new DynamicCheckLink(this);
            }

            var template = this.negated 
                ? "The {0} is not null whereas it must."
                : "The {0} is null whereas it must not.";
            var message = FluentMessage.BuildMessage(template)
                .AddCustomMessage(this.CustomMessage)
                .For(DefaultNamer())
                .On<object>((object)this.value).ToString();

            this.reporter.ReportError(message);
            return new DynamicCheckLink(this);
        }

        private static EntityNamingLogic DefaultNamer()
        {
            var result = new EntityNamingLogic();
            result.SetNameBuilder(() => "dynamic");
            return result;
        }

        /// <summary>
        ///     Checks if the given dynamic has the expected reference.
        /// </summary>
        /// <param name="expected">Expected reference.</param>
        public DynamicCheckLink IsSameReferenceAs(dynamic expected)
        {
            if (this.negated != (object.ReferenceEquals(this.value, expected)))
            {
                return new DynamicCheckLink(this);
            }

            var template = this.negated
                ? "The {0} is the expected reference whereas it must not."
                : "The {0} is not the expected reference.";
            var message = FluentMessage
                .BuildMessage(template)
                .For(DefaultNamer())
                .AddCustomMessage(this.CustomMessage)
                .Expected(expected)
                .Comparison(this.negated ? "different from" : "")
                .And
                .On<object>(this.value).ToString();

            this.reporter.ReportError(message);
            return new DynamicCheckLink(this);
        }

        /// <summary>
        ///     Checks if the given dynamic has the expected value.
        /// </summary>
        /// <param name="expected">
        ///     The expected value. Comparison is done using <see cref="object.Equals(object, object)" />
        /// </param>
        public DynamicCheckLink IsEqualTo(dynamic expected)
        {
            if (this.negated != object.Equals(this.value, expected))
            {
                return new DynamicCheckLink(this);
            }

            var template = this.negated
                ? "The {0} is equal to the {1} whereas it must not."
                : "The {0} is not equal to the {1}.";
            var message = FluentMessage.
                BuildMessage(template).
                For(DefaultNamer()).
                AddCustomMessage(this.CustomMessage).
                Expected(expected).
                Comparison(this.negated ? "different from" : "").
                And.
                On<object>(this.value).
                ToString();

            this.reporter.ReportError(message);
            return new DynamicCheckLink(this);
        }


        /// <inheritdoc cref="IForkableCheck.ForkInstance" />
        public object ForkInstance()
        {
            return new FluentDynamicCheck(this.value, this.reporter);
        }

        /// <summary>
        /// Invert test condition
        /// </summary>
        public FluentDynamicCheck Not
        {
            get
            {
                var ret = new FluentDynamicCheck(this.value, this.reporter)
                {
                    negated = !this.negated,
                    CustomMessage = this.CustomMessage
                };
                return ret;
            }
        }

        internal string CustomMessage { get; set; }
    }
#endif
}