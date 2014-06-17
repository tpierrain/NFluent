// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FloatSpecificCheckExtensions.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    /// Provides specific check methods to be executed on an <see cref="float"/> value.
    /// </summary>
    public static class FloatSpecificCheckExtensions
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        /// <summary>
        /// Determines whether the specified number evaluates to a value that is not a number (NaN).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The current value is a number.</exception>
        public static ICheckLink<ICheck<float>> IsNaN(this ICheck<float> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!float.IsNaN(checker.Value))
                    {
                        var errorMessage = FluentMessage.BuildMessage("The {0} is a number whereas it must not.").For("float value").On(checker.Value).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage("The {0} is not a number (NaN) whereas it must.").For("float value").On(checker.Value).ToString());
        }

        /// <summary>
        /// Determines whether the specified number evaluates to a value that is finite (i.e. not infinity).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The specified number evaluates to a value that is infinite (i.e. equals to infinity).</exception>
        public static ICheckLink<ICheck<float>> IsFinite(this ICheck<float> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (float.IsInfinity(checker.Value))
                    {
                        var errorMessage = FluentMessage.BuildMessage("The {0} is an infinite number whereas it must not.").For("float value").On(checker.Value).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage("The {0} is a finite number whereas it must not.").For("float value").On(checker.Value).ToString());
        }
    }
}
