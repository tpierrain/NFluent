// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MemberCheckExtensions.cs" company="NFluent">
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
#if !DOTNET_35
    using System;
#endif
    using System.Linq.Expressions;
    using Helpers;

    /// <summary>
    /// Hosts extension method related to member checking
    /// </summary>
    public static class MemberCheckExtensions
    {
        /// <summary>
        /// Extracts a sut member to perform checks on it
        /// </summary>
        /// <param name="check">check context</param>
        /// <param name="extractor">extracting expression (usually in the form of x => x.Member).</param>
        /// <typeparam name="T">Type of the original sut</typeparam>
        /// <typeparam name="TM">Type of the extracted member</typeparam>
        /// <returns>a CheckMember instance offering checks on the member</returns>
        public static CheckMember<T, TM> WhichMember<T, TM>(this ICheck<T> check, Expression<Func<T,TM>> extractor)
        {
            var name = ExpressionHelper.GetPropertyNameFromExpression(extractor);
            // scan the extractor to get the message
            return new CheckMember<T, TM>(check, extractor, name);
        }
    }
}