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
        #region Constructors and Destructors

        #region Static Fields

        /// <summary>
        ///     The anonymous type field mask.
        /// </summary>
        private static readonly Regex AnonymousTypeFieldMask;

        /// <summary>
        ///     The auto property mask.
        /// </summary>
        private static readonly Regex AutoPropertyMask;

        #endregion

        /// <summary>
        ///     Initializes static members of the <see cref="ObjectFieldsCheckExtensions" /> class.
        /// </summary>
        static ObjectFieldsCheckExtensions()
        {
            AutoPropertyMask = new Regex("^<(.*)>k_");
            AnonymousTypeFieldMask = new Regex("^<(.*)>(i_|\\z)");
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks that the actual value has fields equals to the expected value ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected value.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual value doesn't have all fields equal to the expected value ones.
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
        ///     Checks that the actual value doesn't have all fields equal to the expected value ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected value.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual value has all fields equal to the expected value ones.
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
        ///     Checks that the actual value has fields equals to the expected value ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected value.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual value doesn't have all fields equal to the expected value ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasFieldsWithSameValues<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var message = CheckFieldEquality(checker, checker.Value, expected, checker.Negated);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        ///     Checks that the actual value doesn't have all fields equal to the expected value ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected value.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual value has all fields equal to the expected value ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasNotFieldsWithSameValues<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = !checker.Negated;

            var message = CheckFieldEquality(checker, checker.Value, expected, negated);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        #endregion

        #region Methods

        private static IEnumerable<FieldMatch> ScanFields(
            object value,
            object expected,
            IList<object> scanned,
            string prefix = null)
        {
            var result = new List<FieldMatch>();

            for (var expectedType = expected.GetType(); expectedType != null; expectedType = expectedType.GetBaseType())
            {
                if (expectedType.IsArray)
                {
                    var array = expected as Array;
                    var actualArray = value as Array;
                    var extendedFieldInfo = new ExtendedFieldInfo(prefix, expectedType, "");
                    extendedFieldInfo.SetFieldValue(value);
                    if (actualArray == null)
                    {
                        result.Add(new FieldMatch(extendedFieldInfo, null));
                    }
                    else
                    {
                        if (actualArray.Length != array.Length)
                        {
                            var actuelFieldInfo = new ExtendedFieldInfo(prefix, value.GetType(), "");
                            actuelFieldInfo.SetFieldValue(value);
                            result.Add(new FieldMatch(extendedFieldInfo,
                                actuelFieldInfo));
                        }
                        else
                        {
                            var fieldType = expectedType.GetElementType();
                            var actualFieldType = actualArray.GetType().GetElementType();
                            for (var i = 0; i < array.Length; i++)
                            {
                                var prefixWithIndex = $"[{i}]";
                                var expectedFieldDescription = new ExtendedFieldInfo(prefix, fieldType, prefixWithIndex);
                                var actualFieldDescription = new ExtendedFieldInfo(prefix, actualFieldType, prefixWithIndex);
                                expectedFieldDescription.SetFieldValue(array.GetValue(i));
                                actualFieldDescription.SetFieldValue(actualArray.GetValue(i));
                                CompareValue(expectedFieldDescription, actualFieldDescription, result, scanned);
                            }
                        }
                    }
                    break;
                }

                foreach (var fieldInfo in expectedType.GetFields(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    | BindingFlags.FlattenHierarchy))
                {
                    var expectedFieldDescription = new ExtendedFieldInfo(prefix, fieldInfo.FieldType, fieldInfo.Name);
                    var fieldType = value?.GetType() ?? fieldInfo.FieldType;
                    var actualFieldMatching = FindField(fieldType, expectedFieldDescription.NameInSource);

                    expectedFieldDescription.SetFieldValue(fieldInfo.GetValue(expected));
                    // field not found in SUT
                    if (actualFieldMatching == null)
                    {
                        result.Add(new FieldMatch(expectedFieldDescription, null));
                        continue;
                    }

                    var actualFieldDescription =
                        new ExtendedFieldInfo(prefix, actualFieldMatching.FieldType, actualFieldMatching.Name);

                    // now, let's get to the values
                    actualFieldDescription.SetFieldValue(actualFieldMatching.GetValue(value));

                    CompareValue(expectedFieldDescription, actualFieldDescription, result, scanned);
                }
            }

            return result;
        }

        private static void CompareValue(ExtendedFieldInfo expectedFieldDescription,
            ExtendedFieldInfo actualFieldDescription,
            List<FieldMatch> result, IList<object> scanned)
        {
            if (expectedFieldDescription.ChecksIfImplementsEqual())
            {
                result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
            }
            else if (!scanned.Contains(expectedFieldDescription.Value))
            {
                scanned.Add(expectedFieldDescription.Value);
                if (expectedFieldDescription.Value == null)
                {
                    result.Add(new FieldMatch(expectedFieldDescription, actualFieldDescription));
                }
                else
                {
                    result.AddRange(
                        ScanFields(
                            actualFieldDescription.Value,
                            expectedFieldDescription.Value,
                            scanned,
                            $"{expectedFieldDescription.LongFieldName}."));
                }
            }
        }

        private static FieldInfo FindField(Type type, string name)
        {
            while (true)
            {
                const BindingFlags FlagsForFields =
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

                var result = type.GetField(name, FlagsForFields);

                if (result != null)
                {
                    return result;
                }

                if (type.GetBaseType() == null)
                {
                    return null;
                }

                // compensate any auto-generated name
                FieldKind fieldKind;
                var actualName = ExtractFieldNameAsInSourceCode(name, out fieldKind);

                foreach (var field in type.GetFields(FlagsForFields))
                {
                    var fieldName = ExtractFieldNameAsInSourceCode(field.Name, out fieldKind);
                    if (fieldName == actualName)
                    {
                        return field;
                    }
                }

                type = type.GetBaseType();
            }
        }

        internal static string ExtractFieldNameAsInSourceCode(string name, out FieldKind kind)
        {
            string result;
            if (EvaluateCriteria(AutoPropertyMask, name, out result))
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

        private static string CheckFieldEquality<T>(IChecker<T, ICheck<T>> checker, object value, object expected,
            bool negated)
        {
            var analysis = ScanFields(value, expected, new List<object>());

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

        #endregion

        private class FieldMatch
        {
            #region Constructors and Destructors

            #endregion

            #region Fields

            private readonly ExtendedFieldInfo actual;

            #endregion

            public FieldMatch(ExtendedFieldInfo expected, ExtendedFieldInfo actual)
            {
                this.actual = actual;
                this.Expected = expected;
            }

            #region Public Properties

            private bool DoValuesMatches
            {
                get
                {
                    if (!this.ExpectedFieldFound)
                    {
                        return false;
                    }

                    if (this.Expected.Value == null)
                    {
                        return this.actual.Value == null;
                    }

                    return this.Expected.Value.Equals(this.actual.Value);
                }
            }

            private ExtendedFieldInfo Expected { get; }

            /// <summary>
            ///     Gets a value indicating whether the expected field has been found.
            /// </summary>
            private bool ExpectedFieldFound => this.actual != null;

            public FluentMessage BuildMessage<T>(IChecker<T, ICheck<T>> checker, bool negated)
            {
                FluentMessage result = null;
                if (this.DoValuesMatches == negated)
                {
                    if (negated)
                    {
                        result =
                            checker.BuildShortMessage(
                                    $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} has the same value in the comparand, whereas it must not.")
                                .For("value");
                        EqualityHelper.FillEqualityErrorMessage(result, this.actual.Value, this.Expected.Value, true,
                            false);
                    }
                    else
                    {
                        if (!this.ExpectedFieldFound)
                        {
                            result = checker.BuildShortMessage(
                                    $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                                .For("value");
                            result.Expected(this.Expected.Value);
                        }
                        else
                        {
                            result =
                                checker.BuildShortMessage(
                                        $"The {{0}}'s {this.Expected.FieldLabel.DoubleCurlyBraces()} does not have the expected value.")
                                    .For("value");
                            EqualityHelper.FillEqualityErrorMessage(result, this.actual.Value, this.Expected.Value,
                                false, false);
                        }
                    }
                }

                return result;
            }

            #endregion
        }

        #region Nested type: ExtendedFieldInfo

        private class ExtendedFieldInfo
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

            public string LongFieldName => this.prefix == null
                ? this.nameInSource
                : $"{this.prefix}{this.nameInSource}";

            public string FieldLabel { get; private set; }

            public string NameInSource => this.nameInSource;

            public object Value { get; private set; }

            #region Public Methods and Operators

            private void ComputeName(string name)
            {
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

            #endregion
        }

        #endregion
    }
}