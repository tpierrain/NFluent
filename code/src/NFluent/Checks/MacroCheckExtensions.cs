// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MacroCheckExtensions.cs" company="NFluent">
//   Copyright 2021 Cyrille DUPUYDAUBY
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

namespace NFluent
{
    /// <summary>
    /// Ho
    /// </summary>
    public static class MacroCheckExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sut"></param>
        /// <param name="macro"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public static MacroContext<T, T1> VerifiesMacro<T, T1>(this ICheck<T> sut, MacroCheck<T, T1> macro)
        {
            return new MacroContext<T, T1>(sut, macro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sut"></param>
        /// <param name="macro"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static MacroContext<T, T1, T2> VerifiesMacro<T, T1, T2>(this ICheck<T> sut, MacroCheck<T, T1, T2> macro)
        {
            return new MacroContext<T, T1, T2>(sut, macro);
        }
    }
}