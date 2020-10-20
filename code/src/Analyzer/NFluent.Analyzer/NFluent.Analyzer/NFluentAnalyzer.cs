using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NFluent.Analyzer
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NFluentAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NFluentAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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
            if (memberAccess.Expression is IdentifierNameSyntax name)
            {
                if (name.ToString() == "Check" && invocationExpression.ArgumentList.Arguments.Any())
                {
                    // we have a 'check.That(x); situation
                    var diagnostic = Diagnostic.Create(Rule, context.Node.GetLocation(), invocationExpression.ArgumentList.Arguments.First().ToString());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
