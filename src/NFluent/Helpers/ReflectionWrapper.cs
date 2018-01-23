#region File header

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedFileInfo.cs" company="">
//   Copyright 2014 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

#endregion

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
#if NETSTANDARD1_3
    using System.Reflection;
#endif
    using System.Text.RegularExpressions;
    using Extensions;
    using static System.String;

    /// <summary>
    /// This class wraps instances for reflection based checks (in NFluent).
    /// </summary>
    public class ReflectionWrapper
    {
        internal string NameInSource { get; }
        private readonly string prefix;
        private readonly string labelPattern;

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

        internal string MemberLongName => IsNullOrEmpty(this.prefix)
            ? this.NameInSource
            : $"{this.prefix}.{this.NameInSource}";

        internal Criteria Criteria { get; set; }

        internal string MemberLabel => Format(this.labelPattern, this.MemberLongName);

        internal object Value { get; }

        internal Type ValueType { get; set; }

        internal bool IsArray => this.ValueType.IsArray;

        internal static ReflectionWrapper BuildFromInstance(Type type, object value, Criteria criteria)
        {
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

            if (EvaluateCriteria(AutoPropertyMask, name, out var nameInSource))
            {
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

            return new ReflectionWrapper(nameInSource, prefix, labelPattern, value?.GetType() ?? type, value, criteria);
        }

        internal static ReflectionWrapper BuildFromProperty(string prefix, string name, Type type, object value,
            Criteria criteria)
        {
            return new ReflectionWrapper(name, prefix, "property '{0}'", value?.GetType() ?? type, value, criteria);
        }

        internal void MapFields(ReflectionWrapper other,
            int depth, Func<ReflectionWrapper, ReflectionWrapper, int, bool> mapFunction)
        {
            this.MapFields(other, new List<object>(), depth, mapFunction);
        }

        private void MapFields(
            ReflectionWrapper actual,
            ICollection<object> scanned,
            int depth, Func<ReflectionWrapper, ReflectionWrapper, int, bool> mapFunction)
        {
            if (this.Value != null)
            {
                if (scanned.Contains(this.Value))
                {
                    return;
                }
                scanned.Add(this.Value);
            }

            if (!mapFunction(this, actual, depth))
            {
                return;
            }

            // we recurse
            foreach (var member in this.GetSubExtendedMemberInfosFields())
            {
                member.MapFields(actual.FindMember(member), scanned, depth - 1, mapFunction);
            }

            // we deal with missing fields
            foreach (var actualFields in actual.GetSubExtendedMemberInfosFields())
            {
                if (this.FindMember(actualFields) == null)
                {
                    mapFunction(null, actualFields, depth);
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
                var fieldType = array.GetType().GetElementType();
                for (var i = 0; i < array.Length; i++)
                {
                    var expectedEntryDescription = BuildFromField(this.MemberLongName, $"[{i}]", fieldType,
                        array.GetValue(i), this.Criteria);
                    result.Add(expectedEntryDescription);
                }
            }
            else
            {
                var memberDico = new Dictionary<string, ReflectionWrapper>();
                var currentType = this.ValueType;
                while (currentType != null)
                {
                    if (this.Criteria.WithFields)
                    {
                        var fieldsInfo = currentType.GetFields(this.Criteria.BindingFlags);
                        foreach (var info in fieldsInfo)
                        {
                            if (memberDico.ContainsKey(info.Name))
                            {
                                continue;
                            }
                            var expectedValue = this.Value == null ? null : info.GetValue(this.Value);
                            var extended = BuildFromField(this.MemberLongName, info.Name, info.FieldType, expectedValue,
                                this.Criteria);
                            memberDico[info.Name] = extended;
                            result.Add(extended);
                        }
                    }

                    if (this.Criteria.WithProperties)
                    {
                        var propertyInfos = currentType.GetProperties(this.Criteria.BindingFlags);
                        foreach (var info in propertyInfos)
                        {
                            if (memberDico.ContainsKey(info.Name))
                            {
                                continue;
                            }
                            var expectedValue = this.Value == null ? null : info.GetValue(this.Value, null);
                            var extended = BuildFromProperty(this.MemberLongName,
                                info.Name, info.PropertyType, expectedValue, this.Criteria);
                            memberDico[info.Name] = extended;
                            result.Add(extended);
                        }
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
        static ReflectionWrapper()
        {
            AutoPropertyMask = new Regex("^<(.*)>k_");
            AnonymousTypeFieldMask = new Regex("^<(.*)>(i_|\\z)");
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
                    isEqual = expected.Value.Equals(actual.Value);
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
    }
}