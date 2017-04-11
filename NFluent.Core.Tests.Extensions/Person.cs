// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="">
//   Copyright 2017 Cyrille Dupuydauby
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
    public class Person
    {
        public string Name { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int Age { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Nationality Nationality { get; set; }

        // ReSharper disable UnusedMember.Local
        private string PrivateHesitation
        {
            // ReSharper restore UnusedMember.Local
            get { return "Kamoulox !"; }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}