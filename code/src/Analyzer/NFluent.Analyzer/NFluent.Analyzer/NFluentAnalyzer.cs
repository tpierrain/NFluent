// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NFluentAnalyzer.cs" company="NFluent">
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
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NFluentAnalyzer : DiagnosticAnalyzer
    {
        public const string MissingCheckId = "NA0001";
        public const string SutIsTheCheckId = "NA0002";
        public const string EnumerationCheckId = "NA0003";
        public const string PreferIsId = "NA0101";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private const string Category = "Testing";

        private static readonly DiagnosticDescriptor MissingCheckRule = BuildRule(MissingCheckId,
            nameof(Resources.MCTitle), nameof(Resources.MCMessageFormat), nameof(Resources.MCDescription), DiagnosticSeverity.Error);

        private static readonly DiagnosticDescriptor SutIsTheCheckRule = BuildRule(SutIsTheCheckId,
            nameof(Resources.SCTitle), nameof(Resources.SCMessageFormat), nameof(Resources.SCDescription));

        private static readonly DiagnosticDescriptor EnumerationCheck = BuildRule(EnumerationCheckId,
            nameof(Resources.CCTitle), nameof(Resources.CCMessageFormat), nameof(Resources.CCDescription));

        private static readonly DiagnosticDescriptor IsEqualToDefaultRule = BuildRule(PreferIsId,
            nameof(Resources.CDTitle), nameof(Resources.CDMessageFormat), nameof(Resources.CDDescription));

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(MissingCheckRule, SutIsTheCheckRule, EnumerationCheck, IsEqualToDefaultRule);

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
            var thatNode = FindInvocationOfThat(context.SemanticModel, statement.Expression).thatExpression;
            var invocationExpression = thatNode;

            if (thatNode == null)
            {
                return;
            }

            // deal for when we have the 'As' variation
            if (thatNode.Parent is MemberAccessExpressionSyntax memberAccess && memberAccess.HasName("As"))
            {
                thatNode = memberAccess.Parent as InvocationExpressionSyntax;
                if (thatNode == null)
                {
                    return;
                }
            }

            var actualCheck = FindActualCheck(thatNode);

            if (actualCheck == null)
            {
                // we have a 'check.That(x); situation
                var diagnostic = Diagnostic.Create(MissingCheckRule, context.Node.GetLocation(),
                    invocationExpression.ArgumentList.Arguments.First().ToString());
                context.ReportDiagnostic(diagnostic);
            }
            else
            {
                var sut = invocationExpression.ArgumentList.Arguments[0].Expression;
                switch (sut)
                {
                    case BinaryExpressionSyntax binaryExpressionSyntax when (actualCheck.HasName("IsTrue") || actualCheck.HasName("IsFalse")):
                    {
                        // we have a Check.That(sut == expected).IsTrue() ==> Check.That(sut).IsEqual(expected);
                        var checkName = BinaryExpressionSutParser(binaryExpressionSyntax, out var realSut, out _);

                        if (!string.IsNullOrEmpty(checkName))
                        {
                            var diagnostic = Diagnostic.Create(SutIsTheCheckRule, context.Node.GetLocation(),
                                realSut.ToString(), checkName);                 
                            context.ReportDiagnostic(diagnostic);
                        }

                        break;
                    }
                    case MemberAccessExpressionSyntax member when actualCheck.HasName("IsEqualTo"):
                    {
                        if (member.HasName("Count"))
                        {
                            // we have a Check.That(sut.Count).IsEqualTo(expected)
                            var sutSymbol = context.SemanticModel.GetSymbolInfo(member.Expression);
                            if (sutSymbol.Symbol.IsCollection())
                            {
                                context.ReportDiagnostic(Diagnostic.Create(EnumerationCheck,
                                    context.Node.GetLocation(),
                                    member.Expression.ToString(), "CountIs"));
                            }
                        }
                        break;
                    }

                    default:
                        if (actualCheck.Parent is InvocationExpressionSyntax fullCheck 
                            && fullCheck.ArgumentList.Arguments.Count == 1
                            && fullCheck.ArgumentList.Arguments[0].Expression is LiteralExpressionSyntax lit
                            && lit.Kind() == SyntaxKind.DefaultLiteralExpression)
                        {
                            // we have a check.that().IsEqualTo(default)
                            context.ReportDiagnostic(Diagnostic.Create(IsEqualToDefaultRule,
                                context.Node.GetLocation()));
                        }
                        break;
                }
            }
        }

        /// <summary>
        ///     Analyze binary expression to extract information required to improve a check
        /// </summary>
        /// <returns>the name of the check to use as a replacement of the binary expression</returns>
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

            string checkName;
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
                default:
                    checkName = string.Empty;
                    break;
            }

            return checkName;
        }

        public static (InvocationExpressionSyntax thatExpression, Version nfluentVersion) FindInvocationOfThat(SemanticModel model,
            SyntaxNode memberAccess)
        {
            foreach (var mAccess in memberAccess.DescendantNodesAndSelf()
                .Where(cn => cn.IsKind(SyntaxKind.SimpleMemberAccessExpression)).Cast<MemberAccessExpressionSyntax>())
            {
                if (!mAccess.HasName("That") || 
                    !(mAccess.Parent is InvocationExpressionSyntax invocation) ||
                    invocation.ArgumentList.Arguments.Count != 1 ||
                    model.GetSymbolInfo(mAccess).Symbol?.ContainingNamespace.Name != "NFluent")
                {
                    continue;
                }

                var symbolNamespace = model.GetSymbolInfo(mAccess).Symbol?.ContainingAssembly;
                var version = symbolNamespace?.Identity.Version ?? new Version(2, 0);
                // we found check.That, let's  find the sut
                return (invocation, version);
            }

            return (null, null);
        }

        public static MemberAccessExpressionSyntax FindActualCheck(InvocationExpressionSyntax thatNode)
        {
            if (!(thatNode.Parent is MemberAccessExpressionSyntax actualCheck))
            {
                return null;
            }   

            // deal for when we have the 'As' and/or 'Not' variation
            if (actualCheck.HasName("Not"))
            {
                actualCheck = actualCheck.Parent as MemberAccessExpressionSyntax;
                if (actualCheck == null)
                {
                    return null;
                }
            }

            if (actualCheck.HasName("As"))
            {
                actualCheck = actualCheck.Parent?.Parent as MemberAccessExpressionSyntax;
            }

            return actualCheck;
        }
    }
}