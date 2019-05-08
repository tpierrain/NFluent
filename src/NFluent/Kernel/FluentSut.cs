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
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35
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
        private readonly T value;
        private readonly EntityNamingLogic namingLogic;

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
            this.namingLogic = new EntityNamingLogic {EntityType = typeof(T)};
        }

        /// <summary>
        /// Builds a new <see cref="FluentSut{T}"/> instance.
        /// </summary>
        /// <param name="other">Value to examine.</param>
        /// <param name="negated">true if the check logic must be negated.</param>
        public FluentSut(FluentSut<T> other, bool negated) : this(other.Value, other.Reporter, negated)
        {
            this.namingLogic = other.namingLogic.Clone();
        }

        /// <summary>
        /// Gets/Sets if the check is negated
        /// </summary>
        public bool Negated { get; protected set; }

        /// <summary>
        /// Sut
        /// </summary>
        public T Value => this.value;

        /// <summary>
        /// Name for the sut.
        /// </summary>
        public EntityNamingLogic SutName => this.namingLogic;

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
        /// <typeparam name="TU">type of the subvalue</typeparam>
        /// <param name="extractor">sub value extractor</param>
        /// <param name="namer">naming methods</param>
        /// <returns>A new <see cref="FluentSut{T}"/> instance</returns>
        public FluentSut<TU> Extract<TU>(Func<T, TU> extractor, Func<FluentSut<T>, string> namer)
        {
            var val = (object)this.value == null ? default(TU) : extractor(this.value);
            var result = new FluentSut<TU>(val, this.Reporter, this.Negated);
            result.SutName.SetNameBuilder(() => namer(this));
            result.CustomMessage = this.CustomMessage;
            return result;
        }
    }
}