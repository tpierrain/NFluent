// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtraFluent.cs" company="">
// //   Copyright 2018 Cyrille Dupuydauby
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

namespace NFluent.ExtraFluent
{
    using System;

    /// <summary>
    /// Provides supplemental/alternative syntax
    /// </summary>
    public static class ExtraFluent
    {
        /// <summary>
        /// Provides entry point to NFluent checks using extension syntax
        /// </summary>
        /// <param name="sut">the system under test</param>
        /// <typeparam name="T">Type of the system under test.</typeparam>
        /// <returns>an Instance of <see cref="ICheck{T}"/></returns>
        // voir GH #253
        [Obsolete("This is an alpha feature. Syntax may change")]
        public static ICheck<T> Verifies<T>(this T sut)
        {
            return Check.That(sut);
        }
    }
}
