// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DictionaryCheckExtension.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
    using System.Linq;

    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IDictionary"/> value.
    /// </summary>
    public static class DictionaryCheckExtension
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
            var checkItem = check as IRunnableCheck<IDictionary>;
            var checkedDico = checkItem.Value;
            var negated = checkItem.Negated;
            string message = null;
            if (checkedDico.Contains(key) == negated)
            {
                if (negated)
                {
                    message =
                        FluentMessage.BuildMessage("The {0} does contain the given key, whereas it must not.")
                                     .For("Dictionary")
                                     .On(checkedDico)
                                     .And.Expected(key)
                                     .Label("Given key:").ToString();
                }
                else
                {
                    message =
                        FluentMessage.BuildMessage("The {0} does not contain the expected key.")
                                     .For("Dictionary")
                                     .On(checkedDico)
                                     .And.Expected(key)
                                     .Label("Expected key:").ToString();   
                }
            }

             if (message != null)
             {
                 throw new FluentCheckException(message);
             }

            return new CheckLink<ICheck<IDictionary>>(check);
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
         /// <param name="value">
         /// The expected value.
         /// </param>
         /// <returns>
         /// A check link.
         /// </returns>
         public static ICheckLink<ICheck<IDictionary>> ContainsValue<K>(this ICheck<IDictionary> check, K value)
         {
             var checkItem = check as IRunnableCheck<IDictionary>;
             var checkedDico = checkItem.Value;
             var negated = checkItem.Negated;
             string message = null;
             var found = false;
             foreach (var item in checkedDico.Values)
             {
                 if (item.Equals(value))
                 {
                     found = true;
                     break;
                 }
             }

             if (found == negated)
             {
                if (negated)
                {
                    message =
                        FluentMessage.BuildMessage("The {0} does contain the given value, whereas it must not.")
                                    .For("Dictionary")
                                    .On(checkedDico)
                                    .And.Expected(value)
                                    .Label("Expected value:").ToString();
                }
                else
                {
                    message =
                        FluentMessage.BuildMessage("The {0} does not contain the expected value.")
                                    .For("Dictionary")
                                    .On(checkedDico)
                                    .And.Expected(value)
                                    .Label("given value:").ToString();
                }
             }

             if (message != null)
             {
                 throw new FluentCheckException(message);
             }

             return new CheckLink<ICheck<IDictionary>>(check);
         }
    }
}