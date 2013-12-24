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
    /// Helper that allow to extract the checker to be used for and from any given fluent check instance.
    /// </summary>
    /// <typeparam name="T">Type of the value to be checked.</typeparam>
    public static class ExtensibilityHelper<T>
    {
        /// <summary>
        /// Extracts the checker to be used in order to check things on the value contained within 
        /// the given fluent check.
        /// </summary>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>The checker to be used to check things on the value contained in the fluent check.</returns>
        public static IChecker<T> ExtractChecker(ICheck<T> check)
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as ICheckForExtensibility<T>).Checker;
        }

        /// <summary>
        /// Extracts the checker to be used in order to check things on the struct instance contained within
        /// the given fluent check.
        /// </summary>
        /// <typeparam name="S">The type of the struct to be checked.</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>
        /// The checker to be used to check things on the value contained in the fluent check.
        /// </returns>
        public static IStructChecker<S> ExtractStructChecker<S>(IStructCheck<S> check) where S : struct
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as IStructCheckForExtensibility<S>).StructChecker;
        }
    }
}