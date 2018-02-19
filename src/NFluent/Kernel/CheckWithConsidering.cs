// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckWithConsidering.cs" company="">
//   Copyright 2018 Cyrille DUPUYDAUBY
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
    using System.Reflection;
    using Helpers;

    internal class CheckWithConsidering: FluentCheck<ReflectionWrapper>, ICheckPlusAnd, IMembersSelection
    {
        public CheckWithConsidering(ReflectionWrapper value, bool negated) : base(value, negated)
        {
        }

        /// <inheritdoc />
        public IFieldsOrProperties Public
        {
            get
            {
                this.Value.Criteria.SetPublic();
                return this;
            }
        }

        /// <inheritdoc />
        public IFieldsOrProperties NonPublic
        {
            get
            {
                this.Value.Criteria.SetNonPublic();
                return this;
            }
        }

        /// <inheritdoc />
        public IFieldsOrProperties All
        {
            get
            {
                this.Value.Criteria.SetNonPublic();
                this.Value.Criteria.SetPublic();
                return this;
            }
        }

        /// <inheritdoc />
        public IPublicOrNot And
        {
            get
            {
                this.Value.Criteria.Reset();
                return this;
            }
        }

        /// <inheritdoc />
        public ICheckPlusAnd Excluding(params string[] field)
        {
            this.Value.Criteria.SetExclusion(field);
            return this;
        }

        /// <inheritdoc />
        public ICheckPlusAnd Fields
        {
            get
            {
                // default to public fields if not specified
                this.Value.Criteria.WithFields = true;
                return this;
            }
        }

        /// <inheritdoc />
        public ICheckPlusAnd Properties
        {
            get
            {
                this.Value.Criteria.WithProperties = true;
                return this;
            }
        }
    }    
}
