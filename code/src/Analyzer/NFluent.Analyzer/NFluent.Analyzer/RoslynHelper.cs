
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
            return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(parameterExpressions.Select( SyntaxFactory.Argument)));
        }
    }
}
