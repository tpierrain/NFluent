// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RoslynHelper.cs" company="NFluent">
//   Copyright 2020 Cyrille DUPUYDAUBY
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

namespace NFluent.Analyzer
{
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class RoslynHelper
    {
        public static bool HasName(this MemberAccessExpressionSyntax memberAccess, string name)
        {
            return memberAccess.Name is IdentifierNameSyntax identifier && identifier.ToString() == name;
        }

        public static ArgumentListSyntax BuildArgumentList(params ExpressionSyntax[] parameterExpressions)
        {
            return SyntaxFactory.ArgumentList(
                SyntaxFactory.SeparatedList(parameterExpressions.Select(SyntaxFactory.Argument)));
        }
    }
}