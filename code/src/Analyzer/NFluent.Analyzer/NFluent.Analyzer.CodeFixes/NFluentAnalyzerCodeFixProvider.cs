// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NFluentAnalyzerCodeFixProvider.cs" company="NFluent">
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
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NFluentAnalyzerCodeFixProvider))]
    [Shared]
    public class NFluentAnalyzerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(NFluentAnalyzer.MissingCheckId, NFluentAnalyzer.SutIsTheCheckId, NFluentAnalyzer.EnumerationCheckId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
            {
                return;
            }

            foreach (var contextDiagnostic in context.Diagnostics)
            {
                var referenceToken = root.FindToken(contextDiagnostic.Location.SourceSpan.Start);
                if (referenceToken.Parent == null)
                {
                    continue;
                }
                var invocationExpression = referenceToken.Parent
                    .AncestorsAndSelf()
                    .OfType<ExpressionStatementSyntax>().First();
                var thatNode =
                    NFluentAnalyzer.FindInvocationOfThat(await context.Document.GetSemanticModelAsync(context.CancellationToken),
                        invocationExpression);
                if (thatNode == null)
                {
                    return;
                }
                switch (contextDiagnostic.Id)
                {
                    case NFluentAnalyzer.MissingCheckId:
                        FixMissingCheck(context, thatNode, contextDiagnostic);
                        break;
                    case NFluentAnalyzer.SutIsTheCheckId:
                        context.RegisterCodeFix(
                            CodeAction.Create(CodeFixResources.ExpandBinaryExpressionTitle,
                                c => ConvertExpressionSut(context.Document, thatNode, c),
                                nameof(CodeFixResources.ExpandBinaryExpressionTitle)),
                            contextDiagnostic);
                        break;
                    case NFluentAnalyzer.EnumerationCheckId:
                        context.RegisterCodeFix(CodeAction.Create(CodeFixResources.SwitchToEnumerableCheck, 
                                c => ConvertEnumerableCheck(context.Document, thatNode, c), 
                                nameof(CodeFixResources.SwitchToEnumerableCheck)), 
                            contextDiagnostic);
                        break;
                }
            }
        }

        private static async Task<Document> ConvertEnumerableCheck(Document contextDocument, InvocationExpressionSyntax thatNode, CancellationToken cancellationToken)
        {
            var sut = thatNode.ArgumentList.Arguments[0].Expression;
            var actualCheck = NFluentAnalyzer.FindActualCheck(thatNode);
            if (!(sut is MemberAccessExpressionSyntax memberAccess) || !memberAccess.HasName("Count") ||
                !actualCheck.HasName("IsEqualTo"))
            {
                return contextDocument;
            }

            // replace Check.That(sut.Count).IsEqualTo(10) by Check.That(sut).CountIs(10)
            var fixedThat = thatNode.Update(thatNode.Expression,
                RoslynHelper.BuildArgumentList(memberAccess.Expression));
            var fixedCheck = actualCheck.Update(actualCheck.Expression.ReplaceNode(thatNode, fixedThat), 
                actualCheck.OperatorToken, SyntaxFactory.IdentifierName("CountIs"));
            var root = await contextDocument.GetSyntaxRootAsync(cancellationToken);
            return root == null ? contextDocument : contextDocument.WithSyntaxRoot(root.ReplaceNode(actualCheck, fixedCheck));
        }

        private static async Task<Document> ConvertExpressionSut(Document contextDocument,
            InvocationExpressionSyntax thatNode, CancellationToken cancellationToken)
        {
            var sut = thatNode.ArgumentList.Arguments[0].Expression;
            
            var actualCheck = NFluentAnalyzer.FindActualCheck(thatNode);

            if (sut is BinaryExpressionSyntax binaryExpressionSyntax &&
                (actualCheck.HasName("IsTrue") || actualCheck.HasName("IsFalse")))
            {
                var checkName =
                    NFluentAnalyzer.BinaryExpressionSutParser(binaryExpressionSyntax, out var realSut,
                        out var refValue);

                // can we fix this ?
                if (!string.IsNullOrEmpty(checkName))
                {
                    // use the 'sut' as 'That's argument
                    ExpressionSyntax altThat = thatNode.Update(thatNode.Expression, RoslynHelper.BuildArgumentList(realSut));

                    if (actualCheck.HasName("IsFalse"))
                    {
                        // inject 'Not'
                        altThat = SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression, altThat,
                            SyntaxFactory.IdentifierName("Not"));
                    }
                    // inject the fixed 'That'
                    var updatedCheckThat = actualCheck.ReplaceNode(thatNode, altThat);
                    // inject the proper check name
                    var altCheckNaMme = updatedCheckThat.Update(updatedCheckThat.Expression, 
                        updatedCheckThat.OperatorToken, 
                        SyntaxFactory.IdentifierName(checkName));
                    // inject the parameter
                    var newFix = ((InvocationExpressionSyntax)actualCheck.Parent)?.Update(altCheckNaMme, RoslynHelper.BuildArgumentList(refValue));
                    if (newFix == null)
                    {
                        // fix generation failed
                        return contextDocument;
                    }
                    // inject the fixed check
                    var root = await contextDocument.GetSyntaxRootAsync(cancellationToken);
                    if (root != null)
                    {
                        return contextDocument.WithSyntaxRoot(root.ReplaceNode(actualCheck.Parent, newFix));
                    }
                }
            }

            return contextDocument;
        }

        private static void FixMissingCheck(CodeFixContext context, InvocationExpressionSyntax thatNode,
            Diagnostic contextDiagnostic)
        {
            if (thatNode.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression &&
                thatNode.ArgumentList.Arguments.Any())
            {
                var memberAccess = (MemberAccessExpressionSyntax) thatNode.Expression;
                if (memberAccess.Expression is IdentifierNameSyntax)
                {
                    context.RegisterCodeFix(
                        CodeAction.Create(CodeFixResources.AddSimpleCheckTitle,
                            c => AddAutomaticCheckMethod(context.Document, thatNode, c),
                            nameof(CodeFixResources.AddSimpleCheckTitle)),
                        contextDiagnostic);
                }
            }
        }

        private static async Task<Document> AddAutomaticCheckMethod(Document document,
            ExpressionSyntax invocationExpression,
            CancellationToken cancellationToken)
        {
            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            var info = semanticModel.GetSymbolInfo(invocationExpression);
            var sutType = ((IMethodSymbol) info.Symbol)?.Parameters[0].Type;
            var replacementNode = BuildCorrectCheckThatExpression(invocationExpression, sutType);

            if (replacementNode == null)
            {
                return document;
            }

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return root == null ? document : document.WithSyntaxRoot(root.ReplaceNode(invocationExpression, replacementNode));
        }

        private static InvocationExpressionSyntax BuildCorrectCheckThatExpression(
            ExpressionSyntax invocationExpression, ITypeSymbol sutType)
        {
            var checkName = string.Empty;

            // deal with well known types
            switch (sutType.SpecialType)
            {
                case SpecialType.System_Boolean:
                    checkName = "IsTrue";
                    // When we have a reference type
                    break;
                case SpecialType.System_String:
                    checkName = "IsNotEmpty";
                    break;
                case SpecialType.System_Enum:
                    break;
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                    checkName = "IsNotZero";
                    break;
                case SpecialType.System_DateTime:
                    break;
                case SpecialType.System_IAsyncResult:
                    break;
                case SpecialType.System_AsyncCallback:
                    break;
                default:
                    if (sutType.TypeKind == TypeKind.Array || sutType.AllInterfaces.Any(t =>
                        t.SpecialType == SpecialType.System_Collections_IEnumerable))
                    {
                        return SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    invocationExpression, SyntaxFactory.IdentifierName("Not")),
                                SyntaxFactory.IdentifierName("IsEmpty")));
                    }

                    if (sutType.IsReferenceType ||
                        sutType.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                    {
                        checkName = "IsNotNull";
                    }

                    break;
            }

            InvocationExpressionSyntax replacementNode;
            if (!string.IsNullOrEmpty(checkName))
            {
                // no fix applied

                replacementNode = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        invocationExpression,
                        SyntaxFactory.IdentifierName(checkName)));
            }
            else
            {
                replacementNode = null;
            }

            return replacementNode;
        }
    }
}