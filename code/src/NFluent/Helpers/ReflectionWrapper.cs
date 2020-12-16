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
    using Extensions;
    using Kernel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

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
            AutoPropertyMask = new Regex("^<(.*)>k_", RegexOptions.Compiled);
            AnonymousTypeFieldMask = new Regex("^<(.*)>(i_|\\z)", RegexOptions.Compiled);
        }

        private ReflectionWrapper(string nameInSource, string prefix, string labelPattern, Type type, object value,
            ClassMemberCriteria criteria)
        {
            this.NameInSource = nameInSource;
            this.prefix = prefix;
            this.labelPattern = labelPattern;
            this.Criteria = criteria;
            this.ValueType = type;
            this.Value = value;
        }

        internal string NameInSource { get; }

        internal string MemberLongName => string.IsNullOrEmpty(this.prefix)
            ? this.NameInSource
            : $"{this.prefix}.{this.NameInSource}";

        internal ClassMemberCriteria Criteria { get; set; }

        internal string MemberLabel => string.Format(this.labelPattern, this.MemberLongName);

        internal object Value { get; }

        internal Type ValueType { get; set; }

        internal bool IsArray => this.ValueType.IsArray;

        internal static ReflectionWrapper BuildFromInstance(Type type, object value, ClassMemberCriteria criteria)
        {
            if (type == typeof(ReflectionWrapper))
            {
                return value as ReflectionWrapper;
            }

            return new ReflectionWrapper(string.Empty, string.Empty, string.Empty, type, value, criteria);
        }

        internal static ReflectionWrapper BuildFromType(Type type, ClassMemberCriteria criteria)
        {
            return new ReflectionWrapper(string.Empty, string.Empty, string.Empty, type, null, criteria);
        }

        internal static ReflectionWrapper BuildFromField(string prefix, string name, Type type, object value,
            ClassMemberCriteria criteria)
        {
            string labelPattern;
            if (EvaluateCriteria(AutoPropertyMask, name, out var nameInSource))
            {
                if (criteria.WithProperties)
                {
                    return null;
                }
                labelPattern = $"autoproperty '{{0}}' (field '{name}')";
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
                criteria);
        }

        internal static ReflectionWrapper BuildFromProperty(string prefix, string name, Type type, object value,
            ClassMemberCriteria criteria)
        {
            return new ReflectionWrapper(name, prefix, "property '{0}'", value?.GetType() ?? type, value, criteria);
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
            if (this.Value != null)
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
            ReflectionWrapper expected,
            ICollection<object> scanned,
            int depth, Func<ReflectionWrapper, ReflectionWrapper, int, bool> mapFunction)
        {
            if (!mapFunction(this, expected, depth))
            {
                // no need to recurse
                return;
            }

            if (this.Value != null)
            {
                // logic recursion prevention, only for (non null) reference type
                if (scanned.Contains(this.Value))
                {
                    return;
                }

                scanned.Add(this.Value);
            }

            // we recurse
            var nextDepth = depth - 1;
            foreach (var member in this.GetSubExtendedMemberInfosFields())
            {
                member.MapFields(expected.FindMember(member), scanned, nextDepth, mapFunction);
            }

            // we deal with missing fields (unless asked to ignore them)
            if (this.Criteria.IgnoreExtra)
            {
                return;
            }

            foreach (var expectedField in expected.GetSubExtendedMemberInfosFields())
            {
                if (this.FindMember(expectedField) == null)
                {
                    mapFunction(null, expectedField, nextDepth);
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
                var array = (Array)this.Value;
                result.AddRange(this.GetSubArrayExtendedInfo(array));
            }
            else
            {
                // TODO: improve support of overloaded fields/properties
                var memberDico = new Dictionary<string, ReflectionWrapper>();
                var currentType = this.ValueType;
                while (currentType != null)
                {
                    var fieldsInfo = currentType.GetFields(this.Criteria.BindingFlagsForFields);
                    result.AddRange(this.ExtractFields(fieldsInfo, memberDico));

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
            finalResult.AddRange(result.Where(member => !this.Criteria.IsNameExcluded(member.NameInSource) && !this.Criteria.IsNameExcluded(member.MemberLongName)));

            return finalResult;
        }

        private IEnumerable<ReflectionWrapper> ExtractProperties(IEnumerable<PropertyInfo> propertyInfos, IDictionary<string, ReflectionWrapper> memberDico)
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

        private IEnumerable<ReflectionWrapper> ExtractFields(IEnumerable<FieldInfo> fieldsInfo, IDictionary<string, ReflectionWrapper> memberDico)
        {
            var result = new List<ReflectionWrapper>();
            foreach (var info in fieldsInfo)
            {
                if (memberDico.ContainsKey(info.Name))
                {
                    continue;
                }

                var expectedValue = info.GetValue(this.Value);
                var extended = BuildFromField(this.MemberLongName, info.Name, info.FieldType, expectedValue,
                    this.Criteria);
                if (extended == null)
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
            var name = this.MemberLongName;
            if (string.IsNullOrEmpty(name))
            {
                name = "this";
            }
            if (array.Rank == 1)
            {
                for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
                {
                    var expectedEntryDescription = BuildFromField(string.Empty, $"{name}[{i}]", fieldType,
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
                        var isLastDimension = j == array.Rank - 1;
                        var currentIndex = isLastDimension ? temp : temp % array.SizeOfDimension(j);
                        label.Append(currentIndex.ToString());
                        label.Append(isLastDimension ? "]" : ",");
                        indices[j] = currentIndex + array.GetLowerBound(j);
                        temp /= array.SizeOfDimension(j);
                    }

                    var expectedEntryDescription = BuildFromField(string.Empty, $"{name}{label}", fieldType,
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

            actualFieldName = string.Empty;
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

            var resultAsText = new StringBuilder(100);
            resultAsText.Append("{ ");
            var first = true;
            foreach (var sub in this.GetSubExtendedMemberInfosFields())
            {
                if (!first)
                {
                    resultAsText.Append(", ");
                }
                else
                {
                    first = false;
                }

                resultAsText.AppendFormat("{0} = {1}", sub.NameInSource, sub.ToString(scanned));
            }

            resultAsText.Append(" }");
            return resultAsText.ToString();
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

                if (depth > 0)
                {
                    return true;
                }

                if (expected.ValueType.ImplementsEquals())
                {
                    result.Add(new MemberMatch(expected, actual));
                    return false;
                }

                if (!expected.IsArray || (actual.IsArray && ((Array)expected.Value).Length == ((Array)actual.Value).Length))
                {
                    if (actual.ValueType.TypeHasMember() || expected.ValueType.TypeHasMember())
                    {
                        return true;
                    }
                }

                result.Add(new MemberMatch(expected, actual));
                return false;
            });
            return result;
        }
    }
}