using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NFluent.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NFluentAnalyzerCodeFixProvider)), Shared]
    public class NFluentAnalyzerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NFluentAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            foreach (var contextDiagnostic in context.Diagnostics)
            {
                var invocationExpression = root.FindToken(contextDiagnostic.Location.SourceSpan.Start).Parent.AncestorsAndSelf()
                    .OfType<InvocationExpressionSyntax>().First();

                if (invocationExpression.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression && invocationExpression.ArgumentList.Arguments.Any())
                {
                    var memberAccess = invocationExpression.Expression as MemberAccessExpressionSyntax;
                    if (memberAccess.Expression is IdentifierNameSyntax)
                    {
                        context.RegisterCodeFix(
                            CodeAction.Create(CodeFixResources.CodeFixTitle, 
                                c => AddAutomaticCheckMethod(context.Document, invocationExpression, c),
                            nameof(CodeFixResources.CodeFixTitle)),
                                contextDiagnostic);
                    }
                }
            }
        }

        private async Task<Document> AddAutomaticCheckMethod(Document document, InvocationExpressionSyntax invocationExpression,
            CancellationToken cancellationToken)
        {
            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            var info = semanticModel.GetSymbolInfo(invocationExpression);
            var sut_type = ((IMethodSymbol) info.Symbol).Parameters[0].Type;
            if (sut_type.IsReferenceType)
            {
                var replacementNode =
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            invocationExpression,
                            SyntaxFactory.IdentifierName(SyntaxFactory.ParseToken("IsNotNull"))));
                var root = await document.GetSyntaxRootAsync(cancellationToken);
                var newRoot = root.ReplaceNode(invocationExpression, replacementNode);
                return document.WithSyntaxRoot(newRoot);
            }

            return document;
        }
    }
}