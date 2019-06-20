// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendableCheckLink.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Kernel
{
    /// <summary>
    /// Implements <see cref="IExtendableCheckLink{T,TU}"/>, providing helpers 
    /// </summary>
    /// <typeparam name="T">
    /// Type managed by this extension.
    /// </typeparam>
    /// <typeparam name="TU">Type of the reference comparand.</typeparam>
    public class ExtendableCheckLink<T, TU> : IExtendableCheckLink<T, TU> where T: class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        private readonly TU originalComparand;
        private readonly T previousCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableCheckLink{T,U}"/> class. 
        /// </summary>
        /// <param name="previousCheck">
        /// The previous fluent check.
        /// </param>
        /// <param name="originalComparand">
        /// Comparand used for the first check.
        /// </param>
        public ExtendableCheckLink(T previousCheck, TU originalComparand)
        {
            this.originalComparand = originalComparand;
            this.previousCheck = previousCheck;
        }

        /// <inheritdoc />
        TU IExtendableCheckLink<T, TU>.OriginalComparand => this.originalComparand;

        /// <inheritdoc />
        T IExtendableCheckLink<T, TU>.AccessCheck => this.previousCheck;

        /// <inheritdoc />
        public T And => ((IForkableCheck) this.previousCheck).ForkInstance() as T;
    }
}