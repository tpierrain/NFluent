// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsCommonHelpers.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent.Extensions
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Common helper methods for the NFluent extension methods.
    /// </summary>
    public static class ExtensionsCommonHelpers
    {
        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormated(this object theObject)
        {
            if (theObject is string)
            {
                return string.Format(@"""{0}""", theObject);
            }
            
            if (theObject is DateTime)
            {
                return ToStringProperlyFormated((DateTime)theObject);
            }

            if (theObject is bool)
            {
                return ToStringProperlyFormated((bool)theObject);
            }

            var ienum = theObject as IEnumerable;
            if (ienum != null)
            {
                return ienum.ToEnumeratedString();
            }

            var type = theObject as Type;
            if (type != null)
            {
                return TypeToStringProperlyFormated(type);
            }

            var exc = theObject as Exception;
            if (exc != null)
            {
                return string.Format("{{{0}}}: '{1}'", exc.GetType().FullName, exc.Message);
            }

            return theObject == null ? "null" : theObject.ToString();
        }

        private static string TypeToStringProperlyFormated(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType().ToStringProperlyFormated() + "[]";
            }

            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(byte))
            {
                return "byte";
            }

            if (type == typeof(char))
            {
                return "char";
            }

            if (type == typeof(short))
            {
                return "short";
            }

            if (type == typeof(ushort))
            {
                return "ushort";
            }

            if (type == typeof(int))
            {
                return "int";
            }

            if (type == typeof(uint))
            {
                return "uint";
            }

            if (type == typeof(long))
            {
                return "long";
            }

            if (type == typeof(ulong))
            {
                return "ulong";
            }

            if (type == typeof(sbyte))
            {
                return "sbyte";
            }

            if (type == typeof(bool))
            {
                return "bool";
            }

            if (type == typeof(float))
            {
                return "float";
            }

            if (type == typeof(double))
            {
                return "double";
            }

            if (type == typeof(decimal))
            {
                return "decimal";
            }

            if (type == typeof(object))
            {
                return "object";
            }

            if (type == typeof(void))
            {
                return "void";
            }

            var arguments = type.GetGenericArguments();
            if (arguments.Length > 0)
            {
                // this is a generic type
                var builder = new StringBuilder();
                var typeRoot = type.Name.Substring(0, type.Name.IndexOf('`'));
                if (typeRoot == "Nullable")
                {
                    // specific case for Nullable
                    return arguments[0].ToStringProperlyFormated() + '?';
                }

                builder.Append(typeRoot);
                builder.Append('<');
                bool first = true;
                foreach (var genType in arguments)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }

                    first = false;
                    builder.Append(genType.ToStringProperlyFormated());
                }

                builder.Append('>');
                return builder.ToString();
            }

            return type.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current DateTime.         
        /// </summary>
        /// <param name="theDateTime">The DateTime.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this DateTime theDateTime)
        {
            // return a ISO-8601 Date format
            return string.Format(CultureInfo.InvariantCulture, "{0:o}, Kind = {1}", theDateTime, theDateTime.Kind);
        }

        /// <summary>
        /// Returns a string that represents the current Boolean.         
        /// </summary>
        /// <param name="theBoolean">The Boolean.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this bool theBoolean)
        {
            // Ensure that boolean values are not localized 
            return theBoolean.ToString(CultureInfo.InvariantCulture);
        }
    }
}