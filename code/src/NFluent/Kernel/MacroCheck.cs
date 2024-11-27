// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MacroCheck.cs" company="NFluent">
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
    using Extensibility;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public interface IMacroCheck<out T, in T1, in T2>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
#pragma warning disable CA1716
        ICheckLink<ICheck<T>> With(T1 p1, T2 p2);
#pragma warning restore CA1716
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MacroCheckBase<T>
    {
        internal string ErrorMessage;
        internal readonly ICheck<T> check;

        internal MacroCheckBase(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        internal MacroCheckBase(MacroCheckBase<T> other, ICheck<T> check)
        {
            this.ErrorMessage = other.ErrorMessage;
            this.check = check;
        }

        internal ICheckLink<ICheck<T>> EvaluateWith(Action<T> evaluator)
        {
            using (Check.StartBatch(this.ErrorMessage))
            {
                evaluator(ExtensibilityHelper.ExtractChecker(this.check).Value);
            }

            return ExtensibilityHelper.BuildCheckLink(this.check);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class MacroCheck<T, T1, T2> : MacroCheckBase<T>, IMacroCheck<T, T1, T2>
    {
        internal readonly Action<T, T1, T2> Evaluator;

        private MacroCheck(MacroCheck<T, T1, T2> macro, ICheck<T> check) : base(macro, check)
        {
            this.Evaluator = macro.Evaluator;
        }

        internal MacroCheck(Action<T, T1, T2> evaluator, string errorMessage) :  base(errorMessage)
        {
            this.Evaluator = evaluator;
        }

        internal IMacroCheck<T, T1, T2> PrepareCheck(ICheck<T> theCheck) => new MacroCheck<T, T1, T2>(this, theCheck);

        ICheckLink<ICheck<T>> IMacroCheck<T, T1, T2>.With(T1 p1, T2 p2)
        {
            return base.EvaluateWith((sut) => this.Evaluator(sut, p1, p2));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public class MacroCheck<T, T1>
    {
        internal readonly Action<T, T1> Evaluator;
        internal readonly string ErrorMessage;

        internal MacroCheck(Action<T, T1> evaluator, string errorMessage)
        {
            this.Evaluator = evaluator;
            this.ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MacroCheck<T>
    {
        internal readonly Action<T> Evaluator;
        internal readonly string ErrorMessage;

        internal MacroCheck(Action<T> evaluator, string errorMessage)
        {
            this.Evaluator = evaluator;
            this.ErrorMessage = errorMessage;
        }
    }
}