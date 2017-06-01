// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Child.cs" company="">
//   Copyright 2014 Thomas Pierrain, Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace NFluent.Tests
{
    /// <summary>
    ///     Dummy class for unit testing purpose.
    /// </summary>
    public class Child : Person
    {
        /// <summary>
        ///     Gets or sets the name of the school where the children goes this year.
        /// </summary>
        /// <value>
        ///     The name of the school where the children goes this year.
        /// </value>
        public string CurrentSchoolName { get; set; }
    }
}