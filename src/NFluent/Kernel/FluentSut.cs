// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FluentSut.cs" company="NFluent">
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

namespace NFluent.Kernel
{
    /// <summary>
    /// Base class that store check context.
    /// </summary>
    /// <typeparam name="T">Type of the SUT</typeparam>
    public class FluentSut<T>: IWithValue<T>, INegated
    {
        internal FluentSut(T value) : this(value, !CheckContext.DefaulNegated)
        {
        }

        internal FluentSut(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
        }

        /// <summary>
        ///     Gets/Sets if the check is negated
        /// </summary>
        public bool Negated { get; protected set; }

        /// <summary>
        ///     Sut
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Name for the sut.
        /// </summary>
        public string SutName {get; set; }
    }
}