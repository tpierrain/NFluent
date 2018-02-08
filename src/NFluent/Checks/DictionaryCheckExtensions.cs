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
#if !PORTABLE
    using System.Collections;
#endif
    using System.Collections.Generic;
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40 && !PORTABLE
    using System.Collections.ObjectModel;
#endif

    using Extensibility;
    using Helpers;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IDictionary{K,V}"/> value.
    /// </summary>
    public static class DictionaryCheckExtensions
    {
        /// <summary>
        /// Checks that the actual <see cref="IDictionary{K,V}"/> contains the expected expectedKey.
        /// </summary>
        /// <typeparam name="TK">
        /// The type of the expectedKey element.
        /// </typeparam>
        /// <typeparam name="TU">Type for values.</typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="key">
        /// The expected expectedKey value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ICheckLink<ICheck<IEnumerable<KeyValuePair<TK, TU>>>> ContainsKey<TK, TU>(this ICheck<IEnumerable<KeyValuePair<TK, TU>>> check, TK key)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    var value = checker.Value;
                    if (value is IDictionary<TK, TU> dico)
                    {
                        if (dico.ContainsKey(key))
                            return;
                    }
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40 && !PORTABLE
                    else if  (value is IReadOnlyDictionary<TK, TU> roDico)
                    {
                        if (roDico.ContainsKey(key))
                            return;
                    }
#endif
                    else
                    {
                        foreach (var keyValuePair in value)
                        {
                            if (EqualityHelper.FluentEquals(keyValuePair.Key, key))
                            {
                                return;
                            }
                        }
                    }

                    var message = checker.BuildMessage("The {0} does not contain the expected key.").Expected(key).Label("Expected key:").ToString();
                    throw new FluentCheckException(message);
                },
                checker.BuildMessage("The {0} does contain the given key, whereas it must not.").Expected(key).Label("Given key:").ToString());
        }


        /// <summary>
        /// Checks that the actual <see cref="IDictionary{K,V}"/> contains the expected value.
        /// </summary>
        /// <typeparam name="TK">
        /// The type of the expectedKey element.
        /// </typeparam>
        /// <typeparam name="TU">
        /// Value type.
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
        public static ICheckLink<ICheck<IEnumerable<KeyValuePair<TK, TU>>>> ContainsValue<TK, TU>(this ICheck<IEnumerable<KeyValuePair<TK, TU>>> check, TU expectedValue)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    var value = checker.Value;
                    foreach (var keyValuePair in value)
                    {
                        if (EqualityHelper.FluentEquals(keyValuePair.Value, expectedValue))
                        {
                            return;
                        }
                    }
                    var message = checker.BuildMessage("The {0} does not contain the expected value.").Expected(expectedValue).Label("Expected value:").ToString();
                    throw new FluentCheckException(message);
                },
                checker.BuildMessage("The {0} does contain the given value, whereas it must not.").Expected(expectedValue).Label("Expected value:").ToString());
        }

        /// <summary>
        /// Checks that the actual <see cref="IDictionary{K,V}"/> contains the expected key-value pair.
        /// </summary>
        /// <typeparam name="TK">The key type.</typeparam>
        /// <typeparam name="TU">The value type.</typeparam>
        /// <param name="check">Fluent check.</param>
        /// <param name="expectedKey">Expected key.</param>
        /// <param name="expectedValue">Expected value.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<IEnumerable<KeyValuePair<TK, TU>>>> ContainsPair<TK, TU>(
            this ICheck<IEnumerable<KeyValuePair<TK, TU>>> check,
            TK expectedKey,
            TU expectedValue)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    var value = checker.Value;
                    var foundValue = default(TU);
                    var found = false;
                    if (value is IDictionary<TK, TU> dico)
                    {
                        found = dico.TryGetValue(expectedKey, out foundValue);
                    }
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40 && !PORTABLE
                    else if  (value is IReadOnlyDictionary<TK, TU> roDico)
                    {
                        found = roDico.TryGetValue(expectedKey, out foundValue);
                    }

#endif
                    else
                    {
                        foreach (var keyValuePair in value)
                        {
                            if (!EqualityHelper.FluentEquals(keyValuePair.Key, expectedKey))
                            {
                                continue;
                            }

                            found = true;
                            foundValue = keyValuePair.Value;
                            break;
                        }
                    }
                    // check found value
                    if (found && EqualityHelper.FluentEquals(foundValue, expectedValue))
                    {
                        return;
                    }

                    var message = checker.BuildMessage(!found ? "The {0} does not contain the expected key-value pair. The given key was not found." : "The {0} does not contain the expected value for the given key.");
                    message.Expected(new KeyValuePair<TK, TU>(expectedKey, expectedValue))
                        .Label("Expected pair:");
                    throw new FluentCheckException(message.ToString());
                },
                checker.BuildMessage("The {0} does contain the given value, whereas it must not.").Expected(expectedValue).Label("Expected value:").ToString());
        }
#if !PORTABLE
        /// <summary>
        /// Checks that the actual <see cref="Hashtable"/> contains the expected expectedKey.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="key">
        /// The expected expectedKey value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ICheckLink<ICheck<Hashtable>> ContainsKey(this ICheck<Hashtable> check, object key)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.ContainsKey(key))
                        {
                            return;
                        }

                        var message = checker.BuildMessage("The {0} does not contain the expected key.").Expected(key).Label("Expected key:").ToString();
                        throw new FluentCheckException(message);
                    },
                checker.BuildMessage("The {0} does contain the given key, whereas it must not.").Expected(key).Label("Given key:").ToString());
        }

        /// <summary>
        /// Checks that the actual <see cref="Hashtable"/> contains the expected value.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expectedValue">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ICheckLink<ICheck<Hashtable>> ContainsValue(this ICheck<Hashtable> check, object expectedValue)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.ContainsValue(expectedValue))
                        {
                            return;
                        }

                        var message = checker.BuildMessage("The {0} does not contain the expected value.").Expected(expectedValue).Label("Expected value:").ToString();
                        throw new FluentCheckException(message);
                    },
                checker.BuildMessage("The {0} does contain the given value, whereas it must not.").Expected(expectedValue).Label("Expected value:").ToString());
        }

        /// <summary>
        /// Checks that the actual <see cref="Hashtable"/> contains the expected key-value pair.
        /// </summary>
        /// <param name="check">Fluent check.</param>
        /// <param name="expectedKey">Expected key.</param>
        /// <param name="expectedValue">Expected value.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<Hashtable>> ContainsPair(
            this ICheck<Hashtable> check,
            object expectedKey,
            object expectedValue)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    var checkedDictionary = checker.Value;
                    var containsKey = checkedDictionary.ContainsKey(expectedKey);
                    string mainText;
                    if (containsKey)
                    { 
                        if (checkedDictionary[expectedKey].Equals(expectedValue))
                        {
                            return;
                        }
                        mainText = "The {0} does not contain the expected value for the given key.";
                    }
                    else
                    {
                        mainText = "The {0} does not contain the expected key-value pair. The given key was not found.";
                    }

                    var message = checker.BuildMessage(mainText);
                    message.Expected(new KeyValuePair<object, object>(expectedKey, expectedValue)).Label("Expected pair:");
                    throw new FluentCheckException(message.ToString());
                },
                checker.BuildMessage("The {0} does contain the given value, whereas it must not.").Expected(expectedValue).Label("Expected value:").ToString());
        }
#endif
    }
}