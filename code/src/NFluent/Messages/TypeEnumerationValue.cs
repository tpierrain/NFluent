// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TypeEnumerationValue.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Messages
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using System.Linq;

    internal class TypeEnumerationValue : ISelfDescriptiveValue
    {
        private readonly IEnumerable<Type> types;

        public TypeEnumerationValue(IEnumerable<Type> types)
        {
            this.types = types;
        }

        public string ValueDescription => $"an instance of these types {this}";

        public override string ToString()
        {
            return  $"{{{string.Join(", ", this.types.Select(x => x.ToStringProperlyFormatted()).ToArray())}}}";
        }
    }
}