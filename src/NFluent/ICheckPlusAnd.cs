// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ICheckPlusAnd.cs" company="NFluent">
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

namespace NFluent
{
    using Helpers;

    /// <summary>
    /// Allows to select multiple scope using 'And' keyword
    /// </summary>
    public interface ICheckPlusAnd : ICheck<ReflectionWrapper>
    {
        /// <summary>
        /// Link to add another scope
        /// </summary>
        IPublicOrNot And { get; }

        /// <summary>
        /// Lists excluded fields
        /// </summary>
        /// <param name="field">field names to be excluded</param>
        /// <returns>Linking object to.</returns>
        ICheckPlusAnd Excluding(params string[] field);
    }
}