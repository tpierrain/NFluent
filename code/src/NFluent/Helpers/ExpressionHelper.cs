// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExpressionHelper.cs" company="NFluent">
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

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal static class ExpressionHelper
    {
        public static string GetPropertyNameFromExpression<T, TM>(Expression<Func<T, TM>> propertyExpression)
        {
            var nameBuilder = new List<string>();
            var scanner = propertyExpression.Body;
            while (scanner is MemberExpression member)
            {
                nameBuilder.Add(member.Member.Name);
                scanner = member.Expression;
            }

            if (scanner.ToString() != propertyExpression.Parameters[0].ToString())
            {
                nameBuilder.Add(scanner.ToString());
            }
            nameBuilder.Reverse();
            return String.Join(".", nameBuilder.ToArray());
        }
    }
}