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

    internal class Criteria
    {
        private List<string> exclusion;
        private bool isPublic;
        private bool isPrivate;
        private BindingFlags bindingFlagsForFields;
        private BindingFlags bindingFlagsForProperties;

        internal Criteria(BindingFlags bindingFlags)
        {
            this.bindingFlagsForFields = bindingFlags;
            this.bindingFlagsForProperties = bindingFlags;
        }

        public void CaptureFields()
        {
            if (this.isPrivate)
            {
                this.bindingFlagsForFields |= BindingFlags.NonPublic;
                if (this.isPublic)
                {
                    this.bindingFlagsForFields |= BindingFlags.Public;
                }
            }
            else
            {
                this.bindingFlagsForFields |= BindingFlags.Public;
            }
        }

        public void CaptureProperties()
        {
            if (this.isPrivate)
            {
                this.bindingFlagsForProperties |= BindingFlags.NonPublic;
                if (this.isPublic)
                {
                    this.bindingFlagsForProperties |= BindingFlags.Public;
                }
            }
            else
            {
                this.bindingFlagsForProperties |= BindingFlags.Public;
            }
        }

        public BindingFlags BindingFlagsForFields => this.bindingFlagsForFields;

        public BindingFlags BindingFlagsForProperties => this.bindingFlagsForProperties;

        public bool IgnoreExtra { get; set; }

        public bool WithProperties => (this.BindingFlagsForProperties & (BindingFlags.Public | BindingFlags.NonPublic))!=0;

        public bool WithFields => (this.BindingFlagsForFields & (BindingFlags.Public | BindingFlags.NonPublic))!=0;

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