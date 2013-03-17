// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IChainableHttpHeaderOrHttpWebResponseFluentAssertion.cs" company="">
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
    /// Provides a way to chain another instance of <see cref="IHttpWebResponseFluentAssertion"/> or <see cref="IHttpHeaderFluentAssertion"/>.
    /// </summary>
    public interface IChainableHttpHeaderOrHttpWebResponseFluentAssertion : IChainableFluentAssertion<IHttpWebResponseFluentAssertion>
    {
        /// <summary>
        /// Chains a new <see cref="IHttpHeaderFluentAssertion"/> instance to the current assertion.
        /// </summary>
        /// <value>
        /// The new fluent assertion instance which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        IHttpHeaderFluentAssertion Which { get; }
    }
}