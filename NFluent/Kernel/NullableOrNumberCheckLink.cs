// // --------------------------------------------------------------------------------------------------------------------
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
namespace NFluent.Kernel
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using Extensibility;

    /// <summary>
    /// Provides a way to chain two <see cref="IForkableCheck"/> instances or to chain.
    /// </summary>
    /// <typeparam name="TN">Number type of the checked nullable.</typeparam>
    internal class NullableOrNumberCheckLink<TN> : INullableOrNumberCheckLink<TN> where TN : struct
    {
        private readonly ICheck<TN?> previousCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableOrNumberCheckLink{TN}" /> class.
        /// </summary>
        /// <param name="previousCheck">The previous fluent check.</param>
        public NullableOrNumberCheckLink(ICheck<TN?> previousCheck)
        {
            this.previousCheck = previousCheck;
        }

        // used only for check discovery by helpers used for documentation generation
        // ReSharper disable once UnusedMember.Local
        private TN? Value { get; set; } //ncrunch: no coverage 

        /// <summary>
        /// Chains a new fluent check on the current one for the nullable value.
        /// </summary>
        /// <value>
        /// The new fluent check instance dedicated to the nullable, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<TN?> And
        {
            get
            {
                var forkableCheck = this.previousCheck as IForkableCheck;
                Debug.Assert(forkableCheck != null, "forkableCheck != null");
                return forkableCheck.ForkInstance() as ICheck<TN?>;
            }
        }

        /// <summary>
        /// Chains a new <see cref="ICheck{N}"/> instance to the current check.
        /// </summary>
        /// <value>
        /// The new fluent check instance dedicated to the nullable Value, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<TN> Which
        {
            get
            {
                var checkForExtensibility = ExtensibilityHelper.ExtractChecker(this.previousCheck);

                if (!checkForExtensibility.Value.HasValue)
                {
                    throw new FluentCheckException(checkForExtensibility.BuildShortMessage("The checked nullable has no value to be checked.").ToString());
                }

                return new FluentCheck<TN>(checkForExtensibility.Value.Value);            
            }
        }
    }
}