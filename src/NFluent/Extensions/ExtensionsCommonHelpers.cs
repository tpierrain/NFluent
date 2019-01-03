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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Common helper methods for the NFluent extension methods.
    /// </summary>
    public static class ExtensionsCommonHelpers
    {
        private const string NullText = "null";
        private static int maximumSizeBeforeTruncation;
        private static int startingLengthForLongString;
        private static int endingLengthForLongString;
        private const string TruncationMarkerText = "...<<truncated>>...";

        private static readonly Dictionary<Type, string> TypeToString = new Dictionary<Type, string>
        {
            {typeof(string), "string" },{typeof(byte), "byte" }, {typeof(sbyte), "sbyte"}, {typeof(char), "char" },
            {typeof(short), "short"}, {typeof(ushort), "ushort"}, {typeof(int), "int" }, {typeof(uint), "uint" },
            {typeof(long), "long" }, {typeof(ulong), "ulong" }, {typeof(float), "float" }, {typeof(double), "double" },
            {typeof(bool), "bool" }, {typeof(decimal), "decimal" }, { typeof(object), "object"}, {typeof(void), "void" }
            };

        private static readonly IList<Type> NumericalTypes = new List<Type> { 
            typeof(byte), 
            typeof(sbyte), 
            typeof(short), 
            typeof(ushort), 
            typeof(int), 
            typeof(uint), 
            typeof(long), 
            typeof(ulong), 
            typeof(double), 
            typeof(float),
            typeof(decimal)
        };

        static ExtensionsCommonHelpers()
        {
            StringTruncationLength = 20*1048;
        }

        internal static int StringTruncationLength
        {
            get => maximumSizeBeforeTruncation;
            set
            {
                if (value < 20)
                {
                    throw new ArgumentException();
                }
                maximumSizeBeforeTruncation = value;
                value -= TruncationMarkerText.Length;
                startingLengthForLongString = (value * 15) / 19;
                endingLengthForLongString = value - startingLengthForLongString;
            }
        }

        /// <summary>
        /// Checks if a type is numerical (i.e: int, double, short, uint...).
        /// </summary>
        /// <param name="type">Type to evaluate.</param>
        /// <returns>true if the type is a numerical type.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNumerical(this Type type)
        {
            return NumericalTypes.Contains(type);
        }

        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormatted(this object theObject)
        {
            if (theObject == null)
            {
                return NullText;
            }

            if (theObject is char)
            {
                return $"'{theObject}'";
            }

            if (theObject is string s)
            {
                return $@"""{TruncateLongString(s)}""";
            }
            
            if (theObject is DateTime time)
            {
                return ToStringProperlyFormatted(time);
            }

            if (theObject is bool b)
            {
                return ToStringProperlyFormatted(b);
            }

            if (theObject is double d)
            {
                return ToStringProperlyFormatted(d);
            }

            if (theObject is float f)
            {
                return ToStringProperlyFormatted(f);
            }

            if (theObject is Array array)
            {
                return ArrayToStringProperlyFormatted(array);
            }
            
            if (theObject is IEnumerable enumerable)
            {
                return enumerable.ToEnumeratedString();
            }

            if (theObject is Type type)
            {
                return TypeToStringProperlyFormatted(type);
            }

            if (theObject is Exception exc)
            {
                return $"{{{exc.GetType().FullName}}}: '{exc.Message}'";
            }

            if (theObject is Stream stream)
            {
                return $"{stream} (Length: {stream.Length})";
            }

            if (theObject is DictionaryEntry entry)
            {
                return $"[{entry.Key}, {entry.Value}]";
            }
            var result = theObject.ToString();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return result == null ? NullText : TruncateLongString(result);
        }

        private static string TruncateLongString(string result)
        {
            if (result.Length > StringTruncationLength)
            {
                result = result.Substring(0, startingLengthForLongString) + TruncationMarkerText
                         + result.Substring(result.Length - endingLengthForLongString);
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
        public static string TypeToStringProperlyFormatted(this Type type, bool shortName = false)
        {
            if (type.IsArray)
            {
                return type.GetElementType().TypeToStringProperlyFormatted() + "[]";
            }

            // try to find the type among primitive types
            if (TypeToString.TryGetValue(type, out var localResult))
            {
                return localResult;
            }

            // is this a generic type
            var arguments = type.GetGenericArguments();
            var name = shortName ? type.Name : type.ToString();

            if (arguments.Length <= 0)
            {
                return name;
            }

            // this is a generic type
            var builder = new StringBuilder();
            var typeRoot = name.Substring(0, name.IndexOf('`'));
            if (IsNullable(type))
            {
                // specific case for Nullable
                return TypeToStringProperlyFormatted(arguments[0], shortName) + '?';
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
                builder.Append(TypeToStringProperlyFormatted(genType, shortName));
            }

            builder.Append('>');
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current DateTime.         
        /// </summary>
        /// <param name="theDateTime">The DateTime.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        private static string ToStringProperlyFormatted(this DateTime theDateTime)
        {
            // return a ISO-8601 Date format
            return string.Format(CultureInfo.InvariantCulture, "{0:o}, Kind = {1}", theDateTime, theDateTime.Kind);
        }

        /// <summary>
        /// Returns a string that represents the current Boolean.         
        /// </summary>
        /// <param name="theBoolean">The Boolean.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        private static string ToStringProperlyFormatted(this bool theBoolean)
        {
            // Ensure that boolean values are not localized 
#if !PORTABLE && !NETSTANDARD1_3
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
        private static string ToStringProperlyFormatted(this double value)
        {
            // Ensure that boolean values are not localized 
#if !PORTABLE
            return value.ToString(CultureInfo.InvariantCulture);
#else
            return value.ToString();
#endif
        }

        private static string ArrayToStringProperlyFormatted(Array array)
        {
            var result = new StringBuilder();
            var indices = new int[array.Rank];
            for (var i = 0; i < array.Length; i++)
            {
                var temp = i;
                var zeroesStrike = true;
                var closingStrikes = true;
                var closing = string.Empty;
                for (var j = array.Rank-1; j >= 0; j--)
                {
                    var currentIndex = temp % array.SizeOfDimension(j);
                    if (currentIndex == 0 && zeroesStrike)
                    {
                        if (j > 0)
                        {
                            result.Append('{');
                        }
                    }
                    else
                    {
                        zeroesStrike = false;
                    }

                    if (currentIndex == array.SizeOfDimension(j) - 1 && closingStrikes)
                    {
                        if (j > 0)
                        {
                            closing += '}';
                        }
                    }
                    else
                    {
                        closingStrikes = false;
                    }

                    indices[j] = currentIndex + array.GetLowerBound(j);
                    temp /= array.SizeOfDimension(j);
                }

                result.Append(ToStringProperlyFormatted(array.GetValue(indices)));
                result.Append(closing);
                if (i != array.Length - 1)
                {
                    result.Append(", ");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current float.         
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        private static string ToStringProperlyFormatted(this float value)
        {
            // Ensure that boolean values are not localized 
#if !PORTABLE
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
#if NETSTANDARD1_3
            if (!type.GetTypeInfo().IsGenericType)
#else
            if (!type.IsGenericType)
#endif
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
            
            return info!=null && info.DeclaringType == type;
        }

        /// <summary>
        /// Gets the base type of the given type.
        /// </summary>
        /// <param name="type">Type to be analyzed.</param>
        /// <returns>The Base Type.</returns>
        /// <remarks>Simplify port to .Net Core.</remarks>
        public static Type GetBaseType(this Type type)
        {
#if NETSTANDARD1_3
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }
        /// <summary>
        /// Checks if a type is a primitive one.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type type)
        {
#if DOTNET_20 || DOTNET_30 || NETSTANDARD1_3
            var primitives = new []
            {
                typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), 
                typeof(ulong), typeof(int*), typeof(uint*), typeof(long*), typeof(ulong*), typeof(char), typeof(double), typeof(float)
            };
            foreach (var primitive in primitives)
            {
                if (type == primitive)
                {
                    return true;
                }
            }
            return false;
 #else
            return type.IsPrimitive;
#endif
        }

        /// <summary>
        /// Checks if a type is class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClass(this Type type)
        {
#if NETSTANDARD1_3
           return type.GetTypeInfo().IsClass;
#else
            return type.IsClass;
#endif
        }

    }
}