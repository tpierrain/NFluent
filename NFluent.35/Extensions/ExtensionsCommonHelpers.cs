﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Common helper methods for the NFluent extension methods.
    /// </summary>
    public static class ExtensionsCommonHelpers
    {
        private const string NullText = "null";

        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormated(this object theObject)
        {           
            if (theObject is char)
            {
                return string.Format("'{0}'", theObject);
            }

            var s = theObject as string;
            if (s != null)
            {
                return string.Format(@"""{0}""", TruncateLongString(s));
            }
            
            if (theObject is DateTime)
            {
                return ToStringProperlyFormated((DateTime)theObject);
            }

            if (theObject is bool)
            {
                return ToStringProperlyFormated((bool)theObject);
            }

            if (theObject is double)
            {
                return ToStringProperlyFormated((double)theObject);
            }

            if (theObject is float)
            {
                return ToStringProperlyFormated((float)theObject);
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

            var result = theObject.ToString();

            if (result == null)
            {
                return NullText;
            }

            return TruncateLongString(result);
        }

        private static string TruncateLongString(string result)
        {
            if (result.Length > 197)
            {
                result = result.Substring(0, 150) + "...<<truncated>>..."
                         + result.Substring(result.Length - 40);
            }

            return result;
        }

        /// <summary>
        /// Returns a string with the type name, as seen in source code.
        /// </summary>
        /// <param name="type">
        /// The type to get the name of.
        /// </param>
        /// <param name="shortName">
        /// If set to <c>true</c> return the name without namespaces.
        /// </param>
        /// <returns>
        /// A string containing the type name.
        /// </returns>
        public static string TypeToStringProperlyFormated(this Type type, bool shortName = false)
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
            var name = shortName ? type.Name : type.ToString();

            if (arguments.Length > 0)
            {
                // this is a generic type
                var builder = new StringBuilder();
                var typeRoot = name.Substring(0, name.IndexOf('`'));
                if (typeRoot == "Nullable" || typeRoot == "System.Nullable")
                {
                    // specific case for Nullable
                    return TypeToStringProperlyFormated(arguments[0], shortName) + '?';
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
                    builder.Append(TypeToStringProperlyFormated(genType, shortName));
                }

                builder.Append('>');
                return builder.ToString();
            }

            return name;
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
#if !(PORTABLE)
            return theBoolean.ToString(CultureInfo.InvariantCulture);
#else
            return theBoolean.ToString();
#endif
        }

        /// <summary>
        /// Returns a string that represents the current double.         
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this double value)
        {
            // Ensure that boolean values are not localized 
#if !(PORTABLE)
            return value.ToString(CultureInfo.InvariantCulture);
#else
            return value.ToString();
#endif
        }

        /// <summary>
        /// Returns a string that represents the current float.         
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this float value)
        {
            // Ensure that boolean values are not localized 
#if !(PORTABLE)
            return value.ToString(CultureInfo.InvariantCulture);
#else
            return value.ToString();
#endif
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">
        /// The type to be evaluated.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            // return Nullable.GetUnderlyingType(type) != null;
            if (!type.IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether the specified type implements Equals.
        /// </summary>
        /// <param name="type">The type to be analyzed.</param>
        /// <returns><c>true</c> is the specified type implements Equals.</returns>
        public static bool ImplementsEquals(this Type type)
        {
            MethodInfo info;
            try
            {
                info = type.GetMethod("Equals");
            }
            catch (AmbiguousMatchException)
            {
                return true;
            }
            
            if (info == null)
            {
                return false;
            }

            // info cannot be null has ALL .Net types inherits Equals from System.Object
            return info.DeclaringType == type;
        }

        /// <summary>
        /// Doubles the curly braces in the string.
        /// </summary>
        /// <returns>The string having curly braces doubled.</returns>
        /// <param name="value">String to correct.</param>
        public static string DoubleCurlyBraces(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
    }
}