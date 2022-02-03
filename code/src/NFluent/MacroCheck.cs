// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IMacroCheck.cs" company="NFluent">
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
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class MacroCheck<T, T1, T2>
    {
        internal readonly Action<T, T1, T2> evaluator;
        internal readonly string errorMessage;

        internal MacroCheck(Action<T, T1, T2> evaluator, string errorMessage)
        {
            this.evaluator = evaluator;
            this.errorMessage = errorMessage;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class MacroCheck<T, T1>
    {
        internal readonly Action<T, T1> evaluator;
        internal readonly string errorMessage;

        internal MacroCheck(Action<T, T1> evaluator, string errorMessage)
        {
            this.evaluator = evaluator;
            this.errorMessage = errorMessage;
        }
    }
}