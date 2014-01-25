﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NullableOrNumberCheckLink.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent
{
    using System.Diagnostics.CodeAnalysis;

    using NFluent.Extensibility;

    /// <summary>
    /// Provides a way to chain two <see cref="IForkableCheck"/> instances or to chain.
    /// </summary>
    /// <typeparam name="N">Number type of the checked nullable.</typeparam>
    internal class NullableOrNumberCheckLink<N> : INullableOrNumberCheckLink<N> where N : struct
    {
        private readonly ICheck<N?> previousCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableOrNumberCheckLink{N}" /> class.
        /// </summary>
        /// <param name="previousCheck">The previous fluent check.</param>
        public NullableOrNumberCheckLink(ICheck<N?> previousCheck)
        {
            this.previousCheck = previousCheck;
        }

        // used only for check discovery by helpers
        private N? Value { get; set; }

        /// <summary>
        /// Chains a new fluent check on the current one for the nullable value.
        /// </summary>
        /// <value>
        /// The new fluent check instance dedicated to the nullable, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<N?> And
        {
            get
            {
                IForkableCheck forkableCheck = this.previousCheck as IForkableCheck;
                return forkableCheck.ForkInstance() as ICheck<N?>;
            }
        }

        /// <summary>
        /// Chains a new <see cref="ICheck{N}"/> instance to the current check.
        /// </summary>
        /// <value>
        /// The new fluent check instance dedicated to the nullable Value, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<N> Which
        {
            get
            {
                var checkForExtensibility = ExtensibilityHelper.ExtractChecker(this.previousCheck);

                if (!checkForExtensibility.Value.HasValue)
                {
                    throw new FluentCheckException("\nThe checked nullable has no value to be checked.");
                }

                return new FluentCheck<N>(checkForExtensibility.Value.Value);            
            }
        }
    }
}