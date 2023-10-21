// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="INegateableCheck.cs" company="">
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
    /// Fluent check that has the ability to be negated via a 'Not' operator.
    /// </summary>
    /// <typeparam name="T">Fluent check type to be negated.</typeparam>
    public interface INegateableCheck<out T> 
    {
        /// <summary>
        /// OnNegate the next check, and the next check only.
        /// </summary>
        /// <value>
        /// The next check negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
#pragma warning disable CA1716
        T Not { get; }
#pragma warning restore CA1716
    }
}