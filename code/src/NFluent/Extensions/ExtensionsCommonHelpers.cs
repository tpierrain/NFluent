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

        static ExtensionsCommonHelpers()
        {
            StringTruncationLength = 20*1048;
            CountOfLineOfDetails = 5;
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

        internal static int CountOfLineOfDetails { get; set; }

        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormatted(this object theObject)
        {
            switch (theObject)
            {
                case null:
                    return NullText;
                case char _:
                    return $"'{theObject}'";
                case string s:
                    return $"\"{TruncateLongString(s)}\"";
                case Criteria c:
                    return $"\"{TruncateLongString(c.ToString())}\"";
                case DateTime time:
                    return string.Format(CultureInfo.InvariantCulture, "{0:o}, Kind = {1}", time, time.Kind);
                case bool b:
                    return b.ToString(CultureInfo.InvariantCulture);
                case double d:
                    return d.ToString(CultureInfo.InvariantCulture);
                case float f:
                    return f.ToString(CultureInfo.InvariantCulture);
                case IEnumerable enumerable:
                    return enumerable.ToEnumeratedString();
                case Type type:
                    return TypeToStringProperlyFormatted(type);
                case Exception exc:
                    return $"{{{exc.GetType().FullName}}}: '{exc.Message}'";
                case Stream stream:
                    return $"{stream} (Length: {stream.Length})";
                case DictionaryEntry entry:
                    return $"[{entry.Key.ToStringProperlyFormatted()}]= {entry.Value.ToStringProperlyFormatted()}";
                case DateTimeOffset timeOffset:
                    return string.Format(CultureInfo.InvariantCulture, "{0:o} {1}{2}", timeOffset.DateTime, timeOffset.Offset.TotalSeconds>=0 ? "+":"" ,timeOffset.Offset);
                default:
                {
                    var result = theObject.ToString();

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    return result == null ? NullText : TruncateLongString(result);
                }
            }
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
                var dim = new StringBuilder();
                dim.Append(type.GetElementType().ToStringProperlyFormatted());
                dim.Append('[');
                dim.Append(',', type.GetArrayRank() - 1);
                dim.Append(']');
                return dim.ToString();
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
            
            return info.DeclaringType == type;
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
#if NETSTANDARD1_3
            if (type == typeof(object))
            {
                return false;
            }
            return TypeToString.ContainsKey(type);
 #else
            return type.IsPrimitive;
#endif
        }
    }
}