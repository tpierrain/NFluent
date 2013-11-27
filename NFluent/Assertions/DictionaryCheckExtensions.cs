// // --------------------------------  ------------------------------------------------------------------------------------
// // <copyright file="DictionaryCheckExtensions.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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
    using System.Collections;

    using NFluent.Extensibility;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IDictionary"/> value.
    /// </summary>
    public static class DictionaryCheckExtensions
    {
        /// <summary>
        /// Checks that the actual <see cref="IDictionary"/> contains the expected key.
        /// </summary>
        /// <typeparam name="K">
        /// The type of the key element.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="key">
        /// The expected key value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ICheckLink<ICheck<IDictionary>> ContainsKey<K>(this ICheck<IDictionary> check, K key)
        {
            var runnableCheck = ExtensibilityHelper<IDictionary>.ExtractChecker(check);

            return runnableCheck.ExecuteCheck(
                () =>
                {
                    if (!runnableCheck.Value.Contains(key))
                    {
                        var message = FluentMessage.BuildMessage("The {0} does not contain the expected key.").For("dictionary").On(runnableCheck.Value).And.Expected(key).Label("Expected key:").ToString();
                        throw new FluentCheckException(message);
                    }
                },
                FluentMessage.BuildMessage("The {0} does contain the given key, whereas it must not.").For("dictionary").On(runnableCheck.Value).And.Expected(key).Label("Given key:").ToString());
        }

        /// <summary>
        /// Checks that the actual <see cref="IDictionary"/> contains the expected value.
        /// </summary>
        /// <typeparam name="K">
        /// The type of the key element.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expectedValue">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ICheckLink<ICheck<IDictionary>> ContainsValue<K>(this ICheck<IDictionary> check, K expectedValue)
        {
            var runnableCheck = ExtensibilityHelper<IDictionary>.ExtractChecker(check);

            return runnableCheck.ExecuteCheck(
                () =>
                {
                    var found = false;
                    foreach (var item in runnableCheck.Value.Values)
                    {
                        if (item.Equals(expectedValue))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        var message = FluentMessage.BuildMessage("The {0} does not contain the expected value.").For("dictionary").On(runnableCheck.Value).And.Expected(expectedValue).Label("given value:").ToString();
                        throw new FluentCheckException(message);
                    }
                },
                FluentMessage.BuildMessage("The {0} does contain the given value, whereas it must not.").For("dictionary").On(runnableCheck.Value).And.Expected(expectedValue).Label("Expected value:").ToString());
        }
    }
}