// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MacroContext.cs" company="NFluent">
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
    public class MacroContextBase<T>
    {
        private readonly ICheck<T> check;

        internal MacroContextBase(ICheck<T> check)
        {
            this.check = check;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="message"></param>
        protected internal void RunMacro(Action<T> invocation, string message)
        {
            using (Check.StartBatch(message))
            {
                invocation(Extensibility.ExtensibilityHelper.ExtractChecker(this.check).Value);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class MacroContext<T, T1, T2> : MacroContextBase<T>
    {
        private readonly MacroCheck<T, T1, T2> macro;

        internal MacroContext(ICheck<T> check, MacroCheck<T, T1, T2> macro) : base(check) => this.macro = macro;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public void With(T1 param1, T2 param2) => RunMacro(sut => this.macro.evaluator(sut, param1, param2), this.macro.errorMessage);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class MacroContext<T, T1> : MacroContextBase<T>
    {
        private readonly MacroCheck<T, T1> macro;

        internal MacroContext(ICheck<T> check, MacroCheck<T, T1> macro) : base(check) => this.macro = macro;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param1"></param>
        public void With(T1 param1) => RunMacro(sut => this.macro.evaluator(sut, param1), this.macro.errorMessage);
    }
}