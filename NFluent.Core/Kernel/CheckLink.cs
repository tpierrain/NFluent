// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CheckLink.cs" company="">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides a way to chain two <see cref="IForkableCheck"/> instances. 
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="IForkableCheck"/> to be chained.</typeparam>
    internal class CheckLink<T> : ICheckLink<T> where T : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        private readonly T newCheckWithSameValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckLink{T}" /> class.
        /// </summary>
        /// <param name="previousCheck">The previous fluent check.</param>
        public CheckLink(IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense previousCheck)
        {
            var forkableCheck = previousCheck as IForkableCheck;
            this.newCheckWithSameValue = forkableCheck.ForkInstance() as T;
        }

        /// <summary>
        /// Links a new fluent check to the current one.
        /// </summary>
        /// <value>
        /// The new fluent check instance which has been linked to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public T And
        {
            get
            {
                return this.newCheckWithSameValue;
            }
        }
    }
}