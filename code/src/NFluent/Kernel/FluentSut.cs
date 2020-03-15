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
#if !DOTNET_35
    using System;
#endif
    using Extensibility;
    using Messages;

    /// <summary>
    /// Base class that store check context.
    /// </summary>
    /// <typeparam name="T">Type of the SUT</typeparam>
    public class FluentSut<T> : IWithValue<T>, INegated
    {
        /// <summary>
        /// Builds a new <see cref="FluentSut{T}"/> instance.
        /// </summary>
        /// <param name="value">Value to examine.</param>
        /// <param name="reporter">Error reporter to use</param>
        /// <param name="negated">true if the check logic must be negated.</param>
        public FluentSut(T value, IErrorReporter reporter, bool negated)
        {
            this.Reporter = reporter;
            this.Value = value;
            this.Negated = negated;
            this.SutName = new EntityNamingLogic {EntityType = typeof(T)};
        }

        /// <summary>
        /// Builds a new <see cref="FluentSut{T}"/> instance.
        /// </summary>
        /// <param name="other">Value to examine.</param>
        /// <param name="negated">true if the check logic must be negated.</param>
        public FluentSut(FluentSut<T> other, bool negated) : this(other.Value, other.Reporter, negated)
        {
            this.SutName = other.SutName.Clone();
        }

        /// <summary>
        /// Gets/Sets if the check is negated
        /// </summary>
        public bool Negated { get; protected set; }

        /// <summary>
        /// Sut
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Name for the sut.
        /// </summary>
        public EntityNamingLogic SutName { get; }

        /// <summary>
        /// Gets the error reporter
        /// </summary>
        public IErrorReporter Reporter { get; }

        /// <summary>
        /// Gets/Sets a custom error message.
        /// </summary>
        public string CustomMessage { get; set; }

        /// <summary>
        /// Build a FluentSut from an extract of the current value.
        /// </summary>
        /// <typeparam name="TU">type of the sub value</typeparam>
        /// <param name="extractor">sub value extractor</param>
        /// <param name="nameCallback">naming methods</param>
        /// <returns>A new <see cref="FluentSut{T}"/> instance</returns>
        public FluentSut<TU> Extract<TU>(Func<T, TU> extractor, Func<FluentSut<T>, string> nameCallback)
        {
            var val = (object)this.Value == null ? default(TU) : extractor(this.Value);
            var result = new FluentSut<TU>(val, this.Reporter, this.Negated);
            result.SutName.SetNameBuilder(() => nameCallback(this));
            result.CustomMessage = this.CustomMessage;
            return result;
        }
    }
}