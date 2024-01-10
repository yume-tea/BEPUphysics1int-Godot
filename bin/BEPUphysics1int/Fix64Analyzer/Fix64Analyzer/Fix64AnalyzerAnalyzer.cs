using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Fix64Analyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class Fix64AnalyzerAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "F64_NUM";

		private static readonly string Title = "Fix64 Analyzer";
		private static readonly string MessageFormat = "Numeric literal error: {0}";
		private static readonly string Description = "Replace numeric literals with Fix64 static constants.";
		private const string Category = "Types";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public IParameterSymbol IParameterSymbol { get; private set; }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.NumericLiteralExpression);
		}		

		private void AnalyzeNode(SyntaxNodeAnalysisContext context)
		{
			LiteralExpressionSyntax literal = (LiteralExpressionSyntax)context.Node;
			TypeInfo typeInfo = context.SemanticModel.GetTypeInfo(literal);

			if (typeInfo.ConvertedType.ToString() == "FixMath.NET.Fix64")
			{
				var diagnostic = Diagnostic.Create(Rule, literal.GetLocation(), "Numeric Literal should be replaced with static constant");
				context.ReportDiagnostic(diagnostic);
			}	
		}
	}
}
