// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IExposingChecker.cs" company="">
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
    using NFluent.Extensibility;

    /// <summary>
    /// Exposes an executable check for this given type. 
    /// </summary>
    /// <typeparam name="T">The type of the data to be checked.</typeparam>
    public interface IExposingChecker<out T>
    {
        /// <summary>
        /// Gets the runnable check to use for checking something on a value of a given type.
        /// </summary>
        /// <value>
        /// The runnable check to use for checking something on a given type.
        /// </value>
        IChecker<T> Checker { get; } 
    }
}