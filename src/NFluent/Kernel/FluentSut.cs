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
    using Extensibility;

    /// <summary>
    /// Base class that store check context.
    /// </summary>
    /// <typeparam name="T">Type of the SUT</typeparam>
    public class FluentSut<T>: IWithValue<T>, INegated
    {
        private readonly T value;

        /// <summary>
        /// Builds a new <see cref="FluentSut{T}"/> instance.
        /// </summary>
        /// <param name="value">Value to examine.</param>
        /// <param name="reporter">Error reporter</param>
        public FluentSut(T value, IErrorReporter reporter) : this(value, reporter, !CheckContext.DefaultNegated)
        {
        }

        /// <summary>
        /// Builds a new <see cref="FluentSut{T}"/> instance.
        /// </summary>
        /// <param name="value">Value to examine.</param>
        /// <param name="reporter">Error reporter to use</param>
        /// <param name="negated">true if the check logic must be negated.</param>
        public FluentSut(T value, IErrorReporter reporter, bool negated)
        {
            this.Reporter = reporter;
            this.value = value;
            this.Negated = negated;
        }

        /// <summary>
        ///     Gets/Sets if the check is negated
        /// </summary>
        public bool Negated { get; protected set; }

        /// <summary>
        ///     Sut
        /// </summary>
        public T Value => this.value;

        /// <summary>
        /// Name for the sut.
        /// </summary>
        public string SutName {get; set; }

        /// <summary>
        /// Gets the error reporter
        /// </summary>
        public IErrorReporter Reporter { get; }

        /// <summary>
        /// Gets/Sets a custom error message.
        /// </summary>
        public string CustomMessage { get; set; }
    }
}