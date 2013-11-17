// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtensibilityHelper.cs" company="">
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
namespace NFluent.Extensibility
{
    /// <summary>
    /// Helper that allow to extract both the value to be checked and the runner to do so from 
    /// any fluent check instance.
    /// </summary>
    /// <typeparam name="T">Type of the value to be checked.</typeparam>
    public class ExtensibilityHelper<T>
    {
        /// <summary>
        /// Extracts the runner to be used in order to check things on the 
        /// value contained in the given fluent check.
        /// </summary>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>The runner to be used to check things on the value contained in the fluent check.</returns>
        public static ICheckRunner<T> ExtractRunnableCheck(ICheck<T> check)
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as IRunnableCheck<T>).Runner;
        }
    }
}