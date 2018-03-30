//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DynamicCheckLink.cs" company="">
//    Copyright 2017 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
namespace NFluent
{
    /// <summary>
    /// Implements the And operator for dynmaic check. Used in conjonction with <see cref="FluentDynamicCheck"/>
    /// </summary>
    public class DynamicCheckLink
    {
        private readonly FluentDynamicCheck masterCheck;

        /// <summary>
        /// Build a new <see cref="FluentDynamicCheck"/> to link.
        /// </summary>
        /// <param name="masterCheck"></param>
        internal DynamicCheckLink(FluentDynamicCheck masterCheck)
        {
            this.masterCheck = masterCheck;
        }

        /// <summary>
        /// Chains a new fluent check to the current one.
        /// </summary>
        /// <value>
        /// The new fluent check instance which has been chained to the previous one.
        /// </value>
        public FluentDynamicCheck And => (FluentDynamicCheck) this.masterCheck.ForkInstance();
    }
}
#endif
