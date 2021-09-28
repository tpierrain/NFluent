// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Criteria.cs" company="NFluent">
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

namespace NFluent.Kernel
{
    using System.Collections.Generic;
    using System.Reflection;

    internal class ClassMemberCriteria
    {
        private List<string> exclusion;
        private bool isPublic;
        private bool isPrivate;

        internal ClassMemberCriteria(BindingFlags bindingFlags)
        {
            this.BindingFlagsForFields = bindingFlags;
            this.BindingFlagsForProperties = bindingFlags;
        }

        public void CaptureFields()
        {
            // Stryker disable once Assignment: Mutation does not alter behaviour
            this.BindingFlagsForFields |= this.GetBindingFlags();
        }

        private BindingFlags GetBindingFlags()
        {
            return (this.isPrivate ? BindingFlags.NonPublic : BindingFlags.Public) 
                   | (this.isPublic ? BindingFlags.Public : 0);
        }

        public void CaptureProperties()

        {
            // Stryker disable once Unary: Mutation does not alter behaviour
            this.BindingFlagsForProperties |= this.GetBindingFlags();
        }

        public BindingFlags BindingFlagsForFields { get; private set; }

        public BindingFlags BindingFlagsForProperties { get; private set; }

        public bool IgnoreExtra { get; set; }

        public bool WithProperties => (this.BindingFlagsForProperties & (BindingFlags.Public | BindingFlags.NonPublic))!=0;

        public bool IsNameExcluded(string name)
        {
            return this.exclusion?.Contains(name) ?? false;
        }

        public void SetExclusion(string[] fields)
        {
            this.exclusion = new List<string>(fields);
        }

        public void SetPublic()
        {
            this.isPublic = true;
        }

        public void SetNonPublic()
        {
            this.isPrivate = true;
        }

        public void Reset()
        {
            this.isPublic = false;
            this.isPrivate = false;
        }
    }
}