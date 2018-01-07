#region File header

// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectFieldsCheckExtensions.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

#endregion

namespace NFluent
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    ///     Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectFieldsCheckExtensions
    {
        private const BindingFlags FlagsForFields =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary>
        ///     The anonymous type field mask.
        /// </summary>
        private static readonly Regex AnonymousTypeFieldMask;

        /// <summary>
        ///     The auto property mask.
        /// </summary>
        private static readonly Regex AutoPropertyMask;

        /// <summary>
        ///     Initializes static members of the <see cref="ObjectFieldsCheckExtensions" /> class.
        /// </summary>
        static ObjectFieldsCheckExtensions()
        {
            AutoPropertyMask = new Regex("^<(.*)>k_");
            AnonymousTypeFieldMask = new Regex("^<(.*)>(i_|\\z)");
        }

        /// <summary>
        ///     Checks that the actual actualValue has fields equals to the expected actualValue ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use HasFieldsWithSameValues instead.")]
        public static ICheckLink<ICheck<object>> HasFieldsEqualToThose(this ICheck<object> check, object expected)
        {
            return HasFieldsWithSameValues(check, expected);
        }

        /// <summary>
        ///     Checks that the actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue has all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use HasNotFieldsWithSameValues instead.")]
        public static ICheckLink<ICheck<object>> HasFieldsNotEqualToThose(this ICheck<object> check, object expected)
        {
            return HasNotFieldsWithSameValues(check, expected);
        }

        /// <summary>
        ///     Checks that the actual actualValue has fields equals to the expected actualValue ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked actualValue.
        /// </typeparam>
        /// <typeparam name="TU">Type of the expected actualValue.</typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasFieldsWithSameValues<T, TU>(this ICheck<T> check, TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var message = CheckFieldEquality(checker, checker.Value, expected, checker.Negated, FlagsForFields);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        ///     Checks that the actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked actualValue.
        /// </typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue has all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasNotFieldsWithSameValues<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = !checker.Negated;

            var message = CheckFieldEquality(checker, checker.Value, expected, negated, FlagsForFields);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        private static IEnumerable<FieldMatch> ScanFields(
            ExtendedFieldInfo actual,
            ExtendedFieldInfo expected,
            IList<object> scanned,
            int depth, BindingFlags flags,
            string prefix = null)
        {
            var result = new List<FieldMatch>();
            var expectedType = expected.GetValueType();

            for (; expectedType != null; expectedType = expectedType.GetBaseType())
            {
                var fieldInfos = expectedType.GetFields(flags);
                if (fieldInfos.Length <= 0)
                {
                    continue;
                }

                foreach (var fieldInfo in fieldInfos)
                {
                    var expectedFieldValue = fieldInfo.GetValue(expected.Value);
                    var expectedFieldDescription = new ExtendedFieldInfo(
                        prefix,
                        expectedFieldValue?.GetType() ?? fieldInfo.FieldType,
                        fieldInfo.Name);
                    var actualFieldMatching = FindFieldInType(actual.GetValueType(), expectedFieldDescription.NameInSource, flags);

                    expectedFieldDescription.SetFieldValue(expectedFieldValue);

                    // field not found in SUT
                    if (actualFieldMatching == null)
                    {
                        result.Add(new FieldMatch(expectedFieldDescription, null));
                        continue;
                    }

                    var fieldActualValue = actualFieldMatching.GetValue(actual.Value);
                    var actualFieldDescription = new ExtendedFieldInfo(
                        prefix,
                        fieldActualValue?.GetType() ?? actualFieldMatching.FieldType,
                        actualFieldMatching.Name);

                    // now, let's get to the values
                    actualFieldDescription.SetFieldValue(fieldActualValue);

                    CompareValue(expectedFieldDescription, actualFieldDescription, result, scanned, depth - 1, flags);
                }
            }

            return result;
        }

        private static void CompareValue(
            ExtendedFieldInfo expectedFieldDescription,
            ExtendedFieldInfo actualFieldDescription,
            List<FieldMatch> result,
            IList<object> scanned,
            int depth, BindingFlags flags)
        {
            if (expectedFieldDescription.Value != null && scanned.Contains(expectedFieldDescription.Value))
            {
                return;
            }
 
            if (depth <= 0 && expectedFieldDescription.ChecksIfImplementsEqual())
            {
                result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
            }
            else
            {
                scanned.Add(expectedFieldDescription.Value);
                if (expectedFieldDescription.Value == null)
                {
                    result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
                }
                else if (actualFieldDescription.Value == null)
                {
                    result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
                }
                else if (expectedFieldDescription.GetValueType().IsArray)
                {
                    var array = (Array)expectedFieldDescription.Value;
                    var actualArray = (Array)actualFieldDescription.Value;
                    if (actualArray.Length != array.Length)
                    {
                        result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
                    }
                    else
                    {
                        var fieldType = array.GetType().GetElementType();
                        var actualFieldType = actualArray.GetType().GetElementType();
                        for (var i = 0; i < array.Length; i++)
                        {
                            var prefixWithIndex = $"[{i}]";
                            var expectedEntryDescription = new ExtendedFieldInfo(
                                expectedFieldDescription.LongFieldName,
                                fieldType,
                                prefixWithIndex);
                            var actualEntryDescription = new ExtendedFieldInfo(
                                expectedEntryDescription.LongFieldName,
                                actualFieldType,
                                prefixWithIndex);
                            expectedEntryDescription.SetFieldValue(array.GetValue(i));
                            actualEntryDescription.SetFieldValue(actualArray.GetValue(i));
                            CompareValue(expectedEntryDescription, actualEntryDescription, result, scanned, depth - 1, flags);
                        }
                    }
                }
                else
                {
                    result.AddRange(
                        ScanFields(
                            actualFieldDescription,
                            expectedFieldDescription,
                            scanned,
                            depth - 1, flags,
                            expectedFieldDescription.LongFieldName));
                }
            }
        }
 
        private static FieldInfo FindFieldInType(Type type, string name, BindingFlags flags)
        {
            while (type != null)
            {
                var result = type.GetField(name, flags);

                if (result != null)
                {
                    return result;
                }

                // compensate any auto-generated name
                var actualName = ExtractFieldNameAsInSourceCode(name, out _);

                foreach (var field in type.GetFields(flags))
                {
                    var fieldName = ExtractFieldNameAsInSourceCode(field.Name, out _);
                    if (fieldName == actualName)
                    {
                        return field;
                    }
                }

                type = type.GetBaseType();
            }

            return null;
        }

        internal static string ExtractFieldNameAsInSourceCode(string name, out FieldKind kind)
        {
            if (EvaluateCriteria(AutoPropertyMask, name, out var result))
            {
                kind = FieldKind.AutoProperty;
                return result;
            }
 
            if (EvaluateCriteria(AnonymousTypeFieldMask, name, out result))
            {
                kind = FieldKind.AnonymousClass;
                return result;
            }

            result = name;
            kind = FieldKind.Normal;
            return result;
        }

        private static string CheckFieldEquality<T, TU>(
            IChecker<T, ICheck<T>> checker,
            T value,
            TU expected,
            bool negated,
            BindingFlags flags)
        {
            var expectedValue = new ExtendedFieldInfo(string.Empty, expected?.GetType() ?? typeof(TU), string.Empty);
            expectedValue.SetFieldValue(expected);
            var actualValue = new ExtendedFieldInfo(string.Empty, value?.GetType() ?? typeof(T), string.Empty);
            actualValue.SetFieldValue(value);

            return CompareFields(checker, negated, flags, expectedValue, actualValue);
        }

        private static string CompareFields<T>(IChecker<T, ICheck<T>> checker, bool negated, BindingFlags flags,
            ExtendedFieldInfo expectedValue, ExtendedFieldInfo actualValue)
        {
            var analysis = new List<FieldMatch>();
            CompareValue(expectedValue, actualValue, analysis, new List<object>(), 1, flags);

            foreach (var fieldMatch in analysis)
            {
                var result = fieldMatch.BuildMessage(checker, negated);
                if (result != null)
                {
                    return result.ToString();
                }
            }

            return null;
        }

        private static bool EvaluateCriteria(Regex expression, string name, out string actualFieldName)
        {
            var regTest = expression.Match(name);
            if (regTest.Groups.Count >= 2)
            {
                actualFieldName = name.Substring(regTest.Groups[1].Index, regTest.Groups[1].Length);
                return true;
            }

            actualFieldName = string.Empty;
            return false;
        }

        private class FieldMatch
        {
            private readonly ExtendedFieldInfo actual;

            public FieldMatch(ExtendedFieldInfo expected, ExtendedFieldInfo actual)
            {
                this.actual = actual;
                this.Expected = expected;
            }

            private bool DoValuesMatches
            {
                get
                {
                    var comparer = new EqualityHelper.EqualityComparer<object>();
                    return this.ExpectedFieldFound && comparer.Equals(this.actual.Value, this.Expected.Value);
                }
            }

            private ExtendedFieldInfo Expected { get; }

            /// <summary>
            ///     Gets a actualValue indicating whether the expected field has been found.
            /// </summary>
            private bool ExpectedFieldFound => this.actual != null;

            public FluentMessage BuildMessage<T>(IChecker<T, ICheck<T>> checker, bool negated)
            {
                FluentMessage result;
                if (this.DoValuesMatches != negated)
                {
                    return null;
                }

                if (negated)
                {
                    result = checker.BuildShortMessage(
                            $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} has the same value in the comparand, whereas it must not.")
                        .For("value");
                    EqualityHelper.FillEqualityErrorMessage(
                        result,
                        this.actual.Value,
                        this.Expected.Value,
                        true,
                        false);
                }
                else if (!this.ExpectedFieldFound)
                {
                    result = checker.BuildShortMessage(
                            $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                        .For("value");
                    result.Expected(this.Expected.Value);
                }
                else
                {
                    result = checker.BuildShortMessage(
                            $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} does not have the expected value.")
                        .For("value");
                    EqualityHelper.FillEqualityErrorMessage(
                        result,
                        this.actual.Value,
                        this.Expected.Value,
                        false,
                        false);
                }

                return result;
            }
        }

        internal class ExtendedFieldInfo
        {
            private readonly Type type;
            private readonly FieldKind kind;
            private readonly string nameInSource;
            private readonly string prefix;

            public ExtendedFieldInfo(string prefix, Type type, string infoName)
            {
                this.prefix = prefix;
                this.type = type;
                if (EvaluateCriteria(AutoPropertyMask, infoName, out this.nameInSource))
                {
                    this.kind = FieldKind.AutoProperty;
                }
                else if (EvaluateCriteria(AnonymousTypeFieldMask, infoName, out this.nameInSource))
                {
                    this.kind = FieldKind.AnonymousClass;
                }
                else
                {
                    this.nameInSource = infoName;
                    this.kind = FieldKind.Normal;
                }

                this.ComputeName(infoName);
            }

            public string LongFieldName => string.IsNullOrEmpty(this.prefix)
                                               ? this.nameInSource
                                               : $"{this.prefix}.{this.nameInSource}";

            public string FieldLabel { get; private set; }

            public string NameInSource => this.nameInSource;

            public object Value { get; private set; }

            public Type GetValueType()
            {
                return this.type;
            }

            private void ComputeName(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    // this is the SUT itself (or expected value)
                    this.FieldLabel = string.Empty;
                    return;
                }

                switch (this.kind)
                {
                    case FieldKind.AnonymousClass:
                        this.FieldLabel = $"field '{this.LongFieldName}'";
                        break;
                    case FieldKind.AutoProperty:
                        this.FieldLabel = $"autoproperty '{this.LongFieldName}' (field '{name}')";
                        break;
                    default:
                        this.FieldLabel = $"field '{this.LongFieldName}'";
                        break;
                }
            }

            public void SetFieldValue(object obj)
            {
                this.Value = obj;
            }

            public bool ChecksIfImplementsEqual()
            {
                return this.type.ImplementsEquals();
            }
        }
    }
}