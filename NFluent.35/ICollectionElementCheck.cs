// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ICollectionElementCheck.cs" company="">
// //   Copyright 2016 Thomas PIERRAIN
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
    /// <summary>
    /// Provides a way to perform checks on collection elements.
    /// </summary>
    /// <typeparam name="T">Type of the collection elements.</typeparam>
    public interface ICollectionElementCheck<T>
    {
        /// <summary>
        /// Gets a <see cref="ICheck{T}"/> instance for the expected element.
        /// </summary>
        ICheck<T> That { get; }
    }
}