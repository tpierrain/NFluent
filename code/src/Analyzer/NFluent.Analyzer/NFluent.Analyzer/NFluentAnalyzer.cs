

namespace NFluent.Analyzer
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NFluentAnalyzer : DiagnosticAnalyzer
    {
        public const string MissingCheckId = "NA0001";
        public const string SutIsTheCheckId = "NA0002";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private const string Category = "Testing";

        private static readonly DiagnosticDescriptor MissingCheckRule = BuildRule(MissingCheckId, nameof(Resources.MCTitle), nameof(Resources.MCMessageFormat), nameof(Resources.MCDescription));
        private static readonly DiagnosticDescriptor SutIsTheCheckRule = BuildRule(SutIsTheCheckId, nameof(Resources.SCTitle), nameof(Resources.SCMessageFormat), nameof(Resources.SCDescription));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MissingCheckRule, SutIsTheCheckRule);

        private static DiagnosticDescriptor BuildRule(string id, string localizableTitle, string localizableFormat,
            string localizableDescription, DiagnosticSeverity severity = DiagnosticSeverity.Warning,
            string category = Category, bool isEnabledByDefault = true)
        {
            return new DiagnosticDescriptor(id, 
                new LocalizableResourceString(localizableTitle, Resources.ResourceManager, typeof(Resources)), 
                new LocalizableResourceString(localizableFormat, Resources.ResourceManager, typeof(Resources)), 
                category,
                severity,
                isEnabledByDefault,
                new LocalizableResourceString(localizableDescription, Resources.ResourceManager, typeof(Resources))
                );
        }

        public override void Initialize(AnalysisContext context)
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSyntaxNodeAction(AnalyzeExpressionStatement, SyntaxKind.ExpressionStatement);
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
        }

        private static void AnalyzeExpressionStatement(SyntaxNodeAnalysisContext context)
        {
            var statement = (ExpressionStatementSyntax) context.Node;
            if (!(statement.Expression is InvocationExpressionSyntax invocationExpression))
            {
                return;
            }

            if (invocationExpression.Expression.Kind() != SyntaxKind.SimpleMemberAccessExpression)
            {
                return;
            }

            var memberAccess = (MemberAccessExpressionSyntax) invocationExpression.Expression;
            if (memberAccess.HasName("As"))
            {
                if (context.SemanticModel.GetSymbolInfo(memberAccess).Symbol.ContainingNamespace.Name != "NFluent")
                {
                    return;
                }

                if (memberAccess.Expression is InvocationExpressionSyntax invocation &&
                    invocation.Expression is MemberAccessExpressionSyntax syntax)
                {
                    invocationExpression = invocation;
                    memberAccess = syntax;
                }
                else
                {
                    return;
                }
            }
            if (memberAccess.HasName("That"))
            {
                if (context.SemanticModel.GetSymbolInfo(memberAccess).Symbol.ContainingNamespace.Name != "NFluent")
                {
                    return;
                }
                // we have a 'check.That(x); situation
                var diagnostic = Diagnostic.Create(MissingCheckRule, context.Node.GetLocation(), invocationExpression.ArgumentList.Arguments.First().ToString());
                context.ReportDiagnostic(diagnostic);
            }
            else
            {
                var thatNode = FindInvocationOfThat(context.SemanticModel, memberAccess);

                if (thatNode == null)
                {
                    return;
                }

                var sut = thatNode.ArgumentList.Arguments[0].Expression;
                var actualCheck = thatNode.Parent as MemberAccessExpressionSyntax;
                if (sut is BinaryExpressionSyntax binaryExpressionSyntax && (actualCheck.HasName("IsTrue") || actualCheck.HasName("IsFalse") ))
                {
                    var checkName = BinaryExpressionSutParser(binaryExpressionSyntax, out var realSut, out var _);

                    if (!string.IsNullOrEmpty(checkName))
                    {
                        var diagnostic = Diagnostic.Create(SutIsTheCheckRule, context.Node.GetLocation(),
                            realSut.ToString(), checkName);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        public static string BinaryExpressionSutParser(BinaryExpressionSyntax binaryExpressionSyntax,
            out ExpressionSyntax realSut, out ExpressionSyntax referenceValue)
        {
            var direct = true;
            realSut = binaryExpressionSyntax.Left;
            referenceValue = binaryExpressionSyntax.Right;
            if (realSut is LiteralExpressionSyntax && !(referenceValue is LiteralExpressionSyntax))
            {
                referenceValue = realSut;
                realSut = binaryExpressionSyntax.Right;
                direct = false;
            }

            var checkName = string.Empty;
            switch (binaryExpressionSyntax.OperatorToken.Kind())
            {
                case SyntaxKind.EqualsEqualsToken:
                    checkName = "IsEqualTo";
                    break;
                case SyntaxKind.ExclamationEqualsToken:
                    checkName = "IsNotEqualTo";
                    break;
                case SyntaxKind.LessThanToken:
                    checkName = direct ? "IsStrictlyLessThan" : "IsStrictlyGreaterThan";
                    break;
                case SyntaxKind.GreaterThanToken:
                    checkName = direct ? "IsStrictlyGreaterThan" : "IsStrictlyLessThan";
                    break;
                case SyntaxKind.LessThanEqualsToken:
                    checkName = direct ? "IsBefore" : "IsAfter";
                    break;                
                case SyntaxKind.GreaterThanEqualsToken:
                    checkName = direct ? "IsAfter" : "IsBefore";
                    break;
            }

            return checkName;
        }

        public static InvocationExpressionSyntax FindInvocationOfThat(SemanticModel model,
            ExpressionSyntax memberAccess)
        {
            InvocationExpressionSyntax thatNode = null;
            foreach (var childNode in memberAccess.DescendantNodesAndSelf())
            {
                if (childNode.IsKind(SyntaxKind.SimpleMemberAccessExpression))
                {
                    var mAccess = (MemberAccessExpressionSyntax) childNode;
                    if (mAccess.HasName("That")
                        && model.GetSymbolInfo(mAccess).Symbol.ContainingNamespace.Name == "NFluent"
                        && mAccess.Parent is InvocationExpressionSyntax invocation
                        && invocation.ArgumentList.Arguments.Count == 1)
                    {
                        // we found check.That, let's  find the sut
                        thatNode = invocation;
                        break;
                    }
                }
            }

            return thatNode;
        }
    }
}
