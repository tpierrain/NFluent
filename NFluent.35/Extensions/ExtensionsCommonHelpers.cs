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

using System.Collections.Generic;

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
        private const int MaximumSizeBeforeTruncation = 197;
        private const int StartingLengthForLongString = 150;
        private const int EndingLengthForLongString = 40;
        private const string TruncationMarkerText = "...<<truncated>>...";

        private static readonly Dictionary<Type, string> TypeToString = new Dictionary<Type, string>()
            {
            {typeof(string), "string" },{typeof(byte), "byte" }, {typeof(sbyte), "sbyte"}, {typeof(char), "char" },
            {typeof(short), "short"}, {typeof(ushort), "ushort"}, {typeof(int), "int" }, {typeof(uint), "uint" },
            {typeof(long), "long" }, {typeof(ulong), "ulong" }, {typeof(float), "float" }, {typeof(double), "double" },
            {typeof(bool), "bool" }, {typeof(decimal), "decimal" }, { typeof(object), "object"}, {typeof(void), "void" }
            };

        private static readonly HashSet<Type> NumericalTypes = new HashSet<Type>()
        { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(double), typeof(float)};


        /// <summary>
        /// Checks if a type is numerical (i.e: int, double, short, uint...)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumerical(Type type)
        {
            return NumericalTypes.Contains(type);
        }

        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormated(this object theObject)
        {
            if (theObject == null)
            {
                return NullText;
            }

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

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return result == null ? NullText : TruncateLongString(result);
        }

        private static string TruncateLongString(string result)
        {
            if (result.Length > MaximumSizeBeforeTruncation)
            {
                result = result.Substring(0, StartingLengthForLongString) + TruncationMarkerText
                         + result.Substring(result.Length - EndingLengthForLongString);
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
                return type.GetElementType().TypeToStringProperlyFormated() + "[]";
            }

            // try to find the type among primitive types
            string localResult;
            if (TypeToString.TryGetValue(type, out localResult))
            {
                return localResult;
            }
            // is this a generic type
            var arguments = type.GetGenericArguments();
            var name = shortName ? type.Name : type.ToString();

            if (arguments.Length <= 0) return name;

            // this is a generic type
            var builder = new StringBuilder();
            var typeRoot = name.Substring(0, name.IndexOf('`'));
            if (IsNullable(type))
            {
                // specific case for Nullable
                return TypeToStringProperlyFormated(arguments[0], shortName) + '?';
            }

            builder.Append(typeRoot);
            builder.Append('<');
            var first = true;
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

        /// <summary>
        /// Returns a string that represents the current DateTime.         
        /// </summary>
        /// <param name="theDateTime">The DateTime.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        private static string ToStringProperlyFormated(this DateTime theDateTime)
        {
            // return a ISO-8601 Date format
            return string.Format(CultureInfo.InvariantCulture, "{0:o}, Kind = {1}", theDateTime, theDateTime.Kind);
        }

        /// <summary>
        /// Returns a string that represents the current Boolean.         
        /// </summary>
        /// <param name="theBoolean">The Boolean.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        private static string ToStringProperlyFormated(this bool theBoolean)
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
        private static string ToStringProperlyFormated(this double value)
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
        private static string ToStringProperlyFormated(this float value)
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