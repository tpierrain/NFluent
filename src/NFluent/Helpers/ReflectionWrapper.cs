// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ReflectionWrapper.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Extensions;
    using Kernel;
    using static System.String;

    /// <summary>
    ///     This class wraps instances for reflection based checks (in NFluent).
    /// </summary>
    public class ReflectionWrapper
    {
        /// <summary>
        ///     The anonymous type field mask.
        /// </summary>
        private static readonly Regex AnonymousTypeFieldMask;

        /// <summary>
        ///     The auto property mask.
        /// </summary>
        private static readonly Regex AutoPropertyMask;

        private readonly string labelPattern;
        private readonly string prefix;

        /// <summary>
        ///     Initializes static members of the <see cref="ObjectFieldsCheckExtensions" /> class.
        /// </summary>
        static ReflectionWrapper()
        {
            AutoPropertyMask = new Regex("^<(.*)>k_");
            AnonymousTypeFieldMask = new Regex("^<(.*)>(i_|\\z)");
        }

        private ReflectionWrapper(string nameInSource, string prefix, string labelPattern, Type type, object value,
            Criteria criteria)
        {
            this.NameInSource = nameInSource;
            this.prefix = prefix;
            this.labelPattern = labelPattern;
            this.Criteria = criteria;
            this.ValueType = type;
            this.Value = value;
        }

        internal string NameInSource { get; }

        internal string MemberLongName => IsNullOrEmpty(this.prefix)
            ? this.NameInSource
            : $"{this.prefix}.{this.NameInSource}";

        internal Criteria Criteria { get; set; }

        internal string MemberLabel => Format(this.labelPattern, this.MemberLongName);

        internal object Value { get; }

        internal Type ValueType { get; set; }

        internal bool IsArray => this.ValueType.IsArray;

        internal bool IsProperty { get; private set; }

        internal static ReflectionWrapper BuildFromInstance(Type type, object value, Criteria criteria)
        {
            if (type == typeof(ReflectionWrapper))
            {
                return value as ReflectionWrapper;
            }

            return new ReflectionWrapper(Empty, Empty, "instance", type, value, criteria);
        }

        internal static ReflectionWrapper BuildFromType(Type type, Criteria criteria)
        {
            return new ReflectionWrapper(Empty, Empty, "instance", type, null, criteria);
        }

        internal static ReflectionWrapper BuildFromField(string prefix, string name, Type type, object value,
            Criteria criteria)
        {
            string labelPattern;
            var isProperty = false;
            string nameInSource;
            if (EvaluateCriteria(AutoPropertyMask, name, out nameInSource))
            {
                labelPattern = $"autoproperty '{{0}}' (field '{name}')";
                isProperty = true;
            }
            else if (EvaluateCriteria(AnonymousTypeFieldMask, name, out nameInSource))
            {
                labelPattern = "field '{0}'";
            }
            else
            {
                nameInSource = name;
                labelPattern = "field '{0}'";
            }

            return new ReflectionWrapper(nameInSource, prefix, labelPattern, value?.GetType() ?? type, value,
                criteria) {IsProperty = isProperty};
        }

        internal static ReflectionWrapper BuildFromProperty(string prefix, string name, Type type, object value,
            Criteria criteria)
        {
            return new ReflectionWrapper(name, prefix, "property '{0}'", value?.GetType() ?? type, value, criteria)
            {
                IsProperty = true
            };
        }

        internal void MapFields(ReflectionWrapper other,
            int depth, Func<ReflectionWrapper, ReflectionWrapper, int, bool> mapFunction)
        {
            this.MapFields(other, new List<object>(), depth, mapFunction);
        }

        internal void ScanFields(Func<ReflectionWrapper, int, bool> scanField)
        {
            this.ScanFields(scanField, 1, new List<object>());
        }

        private void ScanFields(Func<ReflectionWrapper, int, bool> scanField, int depth, ICollection<object> scanned)
        {
            if (this.ValueType.IsClass() && this.Value != null)
            {
                if (scanned.Contains(this.Value))
                {
                    return;
                }

                scanned.Add(this.Value);
            }

            if (!scanField(this, depth))
            {
                return;
            }

            // we recurse
            foreach (var member in this.GetSubExtendedMemberInfosFields())
            {
                member.ScanFields(scanField, depth - 1, scanned);
            }
        }

        private void MapFields(
            ReflectionWrapper actual,
            ICollection<object> scanned,
            int depth, Func<ReflectionWrapper, ReflectionWrapper, int, bool> mapFunction)
        {
            if (this.ValueType.IsClass() && this.Value != null)
            {
                // logic recursion prevention, only for (non null) reference type
                if (scanned.Contains(this.Value))
                {
                    return;
                }

                scanned.Add(this.Value);
            }

            if (!mapFunction(this, actual, depth))
            {
                // no need to recurse
                return;
            }

            // we recurse
            foreach (var member in this.GetSubExtendedMemberInfosFields())
            {
                member.MapFields(actual.FindMember(member), scanned, depth - 1, mapFunction);
            }

            // we deal with missing fields
            if (this.Criteria.IgnoreExtra)
            {
                return;
            }

            foreach (var actualFields in actual.GetSubExtendedMemberInfosFields())
            {
                if (this.FindMember(actualFields) == null)
                {
                    mapFunction(null, actualFields, depth - 1);
                }
            }
        }

        private IEnumerable<ReflectionWrapper> GetSubExtendedMemberInfosFields()
        {
            var result = new List<ReflectionWrapper>();
            if (this.ValueType.IsPrimitive())
            {
                return result;
            }

            if (this.IsArray)
            {
                var array = (Array) this.Value;
                result.AddRange(this.GetSubArrayExtendedInfo(array));
            }
            else
            {
                // TODO: improve support of overloaded fields/properties
                var memberDico = new Dictionary<string, ReflectionWrapper>();
                var currentType = this.ValueType;
                while (currentType != null)
                {
                    if (this.Criteria.WithFields)
                    {
                        var fieldsInfo = currentType.GetFields(this.Criteria.BindingFlagsForFields);
                        result.AddRange(this.ExtractFields(fieldsInfo, memberDico));
                    }

                    if (this.Criteria.WithProperties)
                    {
                        var propertyInfos = currentType.GetProperties(this.Criteria.BindingFlagsForProperties);
                        result.AddRange(this.ExtractProperties(propertyInfos, memberDico));
                    }

                    currentType = currentType.GetBaseType();
                }
            }

            // scan
            var finalResult = new List<ReflectionWrapper>(result.Count);
            foreach (var member in result)
            {
                if (this.Criteria.IsNameExcluded(member.NameInSource) ||
                    this.Criteria.IsNameExcluded(member.MemberLongName))
                {
                    continue;
                }

                finalResult.Add(member);
            }

            return finalResult;
        }

        private IEnumerable<ReflectionWrapper> ExtractProperties(PropertyInfo[] propertyInfos, Dictionary<string, ReflectionWrapper> memberDico)
        {
            var result = new List<ReflectionWrapper>();
            foreach (var info in propertyInfos)
            {
                if (memberDico.ContainsKey(info.Name) || info.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                var expectedValue = this.Value == null ? null : info.GetValue(this.Value, null);
                var extended = BuildFromProperty(this.MemberLongName,
                    info.Name, info.PropertyType, expectedValue, this.Criteria);
                memberDico[info.Name] = extended;
                result.Add(extended);
            }

            return result;
        }

        private IEnumerable<ReflectionWrapper> ExtractFields(FieldInfo[] fieldsInfo, Dictionary<string, ReflectionWrapper> memberDico)
        {
            var result = new List<ReflectionWrapper>();
            foreach (var info in fieldsInfo)
            {
                if (memberDico.ContainsKey(info.Name))
                {
                    continue;
                }

                var expectedValue = this.Value == null ? null : info.GetValue(this.Value);
                var extended = BuildFromField(this.MemberLongName, info.Name, info.FieldType, expectedValue,
                    this.Criteria);
                if (this.Criteria.WithProperties && extended.IsProperty)
                {
                    continue;
                }

                memberDico[info.Name] = extended;
                result.Add(extended);
            }

            return result;
        }

        private IEnumerable<ReflectionWrapper> GetSubArrayExtendedInfo(Array array)
        {
            var result = new List<ReflectionWrapper>();
            var fieldType = array.GetType().GetElementType();
            if (array.Rank == 1)
            {
                for (var i = array.GetLowerBound(0); i < array.GetUpperBound(0); i++)
                {
                    var expectedEntryDescription = BuildFromField(this.MemberLongName, $"[{i}]", fieldType,
                        array.GetValue(i), this.Criteria);
                    result.Add(expectedEntryDescription);
                }
            }
            else
            {
                var indices = new int[array.Rank];
                for (var i = 0; i < array.Length; i++)
                {
                    var temp = i;
                    var label = new StringBuilder("[");
                    for (var j = 0; j < array.Rank; j++)
                    {
                        var currentIndex = temp % array.SizeOfDimension(j);
                        label.Append(currentIndex.ToString());
                        label.Append(j < array.Rank - 1 ? "," : "]");
                        indices[j] = currentIndex + array.GetLowerBound(j);
                        temp /= array.SizeOfDimension(j);
                    }

                    var expectedEntryDescription = BuildFromField(this.MemberLongName, label.ToString(), fieldType,
                        array.GetValue(indices), this.Criteria);
                    result.Add(expectedEntryDescription);
                }
            }

            return result;
        }

        private ReflectionWrapper FindMember(ReflectionWrapper other)
        {
            var fields = this.GetSubExtendedMemberInfosFields();
            foreach (var info in fields)
            {
                if (other.NameInSource == info.NameInSource)
                {
                    return info;
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

            actualFieldName = Empty;
            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ToString(new List<object>());
        }

        private string ToString(ICollection<object> scanned)
        {
            if (this.Value == null)
            {
                return "null";
            }

            if (scanned.Contains(this.Value))
            {
                return "...";
            }

            scanned.Add(this.Value);

            if (this.ValueType.IsPrimitive())
            {
                return this.Value.ToString();
            }

            var texte = new StringBuilder(100);
            texte.Append("{ ");
            var first = true;
            foreach (var sub in this.GetSubExtendedMemberInfosFields())
            {
                if (!first)
                {
                    texte.Append(", ");
                }
                else
                {
                    first = false;
                }

                texte.AppendFormat("{0} = {1}", sub.NameInSource, sub.ToString(scanned));
            }

            texte.Append(" }");
            return texte.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = BuildFromInstance(obj?.GetType() ?? typeof(object), obj, this.Criteria);
            var isEqual = true;
            this.MapFields(other, 1, (expected, actual, depth) =>
            {
                if (!isEqual || actual == null)
                {
                    // we have established this is not equal
                    return false;
                }

                if (depth <= 0 && expected.ValueType.ImplementsEquals())
                {
                    isEqual = EqualityHelper.FluentEquals(expected.Value, actual.Value);
                    return false;
                }

                return true;
            });
            return isEqual;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (this.Value == null)
            {
                return 1;
            }

            var hasSub = false;
            var subs = this.GetSubExtendedMemberInfosFields();
            var hash = 0;

            foreach (var memberInfosField in subs)
            {
                hash = hash * 23 + memberInfosField.GetHashCode();
                hasSub = true;
            }

            if (!hasSub)
            {
                hash = this.Value.GetHashCode();
            }

            return hash;
        }

        internal List<MemberMatch> MemberMatches<TU>(TU expectedValue)
        {
            var expectedWrapped =
                BuildFromInstance(expectedValue?.GetType() ?? typeof(TU), expectedValue, this.Criteria);

            var result = new List<MemberMatch>();
            expectedWrapped.MapFields(this, 1, (expected, actual, depth) =>
            {
                if (actual?.Value == null || expected?.Value == null)
                {
                    result.Add(new MemberMatch(expected, actual));
                    return false;
                }

                if (depth <= 0 && expected.ValueType.ImplementsEquals())
                {
                    result.Add(new MemberMatch(expected, actual));
                    return false;
                }

                if (!expected.IsArray ||  (actual.IsArray && (((Array) expected.Value).Length == ((Array) actual.Value).Length)))
                {
                    return true;
                }

                result.Add(new MemberMatch(expected, actual));
                return false;
            });
            return result;
        }
    }
}