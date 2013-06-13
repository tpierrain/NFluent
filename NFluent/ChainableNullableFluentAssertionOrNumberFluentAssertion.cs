// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ChainableNullableFluentAssertionOrNumberFluentAssertion.cs" company="">
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

    /// <summary>
    /// Provides a way to chain two <see cref="IForkableFluentAssertion"/> instances or to chain.
    /// </summary>
    /// <typeparam name="N">Number type of the checked nullable.</typeparam>
    internal class ChainableNullableFluentAssertionOrNumberFluentAssertion<N> : IChainableNullableFluentAssertionOrNumberFluentAssertion<N> where N : struct
    {
        private readonly IFluentAssertion<N?> previousFluentAssertion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainableNullableFluentAssertionOrNumberFluentAssertion{N}" /> class.
        /// </summary>
        /// <param name="previousFluentAssertion">The previous fluent assertion.</param>
        public ChainableNullableFluentAssertionOrNumberFluentAssertion(IFluentAssertion<N?> previousFluentAssertion)
        {
            this.previousFluentAssertion = previousFluentAssertion;
        }

        // used only for check discovery by helpers
        private N? Value { get; set; }

        /// <summary>
        /// Chains a new fluent assertion on the current one for the nullable value.
        /// </summary>
        /// <value>
        /// The new fluent assertion instance dedicated to the nullable, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public IFluentAssertion<N?> And
        {
            get
            {
                return this.previousFluentAssertion.ForkInstance() as IFluentAssertion<N?>;
            }
        }

        /// <summary>
        /// Chains a new <see cref="IFluentAssertion{N}"/> instance to the current assertion.
        /// </summary>
        /// <value>
        /// The new fluent assertion instance dedicated to the nullable Value, which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public IFluentAssertion<N> Which
        {
            get
            {
                IRunnableAssertion<N?> runnableAssertion = this.previousFluentAssertion as IRunnableAssertion<N?>;

                if (!runnableAssertion.Value.HasValue)
                {
                    throw new FluentAssertionException("\nThe checked nullable has no value to be checked.");
                }

                return new FluentAssertion<N>(runnableAssertion.Value.Value);            
            }
        }
    }
}