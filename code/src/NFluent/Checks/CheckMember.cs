// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CheckMember.cs" company="NFluent">
//   Copyright 2020 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent
{
    using System;
    using Kernel;

    /// <summary>
    /// Offers checks on members of an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TM"></typeparam>
    public class CheckMember<T, TM>
    {
        private readonly ICheck<T> originalCheck;
        private readonly TM value;

        internal CheckMember(ICheck<T> originalCheck, TM value)
        {
            this.originalCheck = originalCheck;
            this.value = value;
        }

        public CheckMember<T, TM> Verifies(Action<ICheck<TM>> func)
        {
            func(new FluentCheck<TM>(this.value));
            return this;
        }
    }
}