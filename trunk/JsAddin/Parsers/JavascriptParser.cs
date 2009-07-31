using System.Collections.Generic;
using JS_addin.Addin.Code;
using JS_addin.Addin.Helpers;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;

namespace JS_addin.Addin.Parsers
{
	/// <summary>
	/// The js parser.
	/// </summary>
	public static class JavascriptParser
	{
		/// <summary>
		/// The parse.
		/// </summary>
		/// <param name="script">
		/// The js script.
		/// </param>
		/// <returns>
		/// Hierarhy with code structure.
		/// </returns>
		public static Hierachy<CodeNode> Parse(string script)
		{
			var parser = new Parser(script.ToCharArray(), true);
			var comments = new List<Comment>();
			var bindingInfo = new BindingInfo();
			DList<Statement, BlockStatement> sourceElements = parser.ParseProgram(ref comments, ref bindingInfo);
			var nodes = new Hierachy<CodeNode>(new CodeNode { Alias = "All" });

			CreateNodes(nodes, sourceElements);
			return nodes;
		}

		/// <summary>
		/// The create nodes.
		/// </summary>
		/// <param name="statements">
		/// The statements.
		/// </param>
		/// <param name="nodes">
		/// The nodes.
		/// </param>
		private static void CreateNodes(Hierachy<CodeNode> nodes, DList<Statement, BlockStatement> statements)
		{
			var iterator = new DList<Statement, BlockStatement>.Iterator(statements);
			while (iterator.Element != null)
			{
				Statement statement = iterator.Element;
				if (statement != null)
				{
					ProcessStatement(nodes, statement);
				}

				iterator.Advance();
			}
		}

		private static string ConcatAlias(string exist, string appender)
		{
			if (string.IsNullOrEmpty(exist))
			{
				return appender;
			}

			return exist + "." + appender;
		}

		private static void ProcessExpression(Hierachy<CodeNode> nodes, Expression exp, string expressionAlias)
		{
			if (exp is BinaryOperatorExpression)
			{
				var bexp = (BinaryOperatorExpression) exp;

				string alias = null;
				if (bexp.Left is QualifiedExpression)
				{
					alias = ((QualifiedExpression)bexp.Left).Qualifier.Spelling;
				}

				ProcessExpression(nodes, bexp.Right, ConcatAlias(expressionAlias, alias));
				return;
			}

			if (exp is InvocationExpression)
			{
				var invexp = (InvocationExpression) exp;
				ProcessExpression(nodes, invexp.Target, ConcatAlias(expressionAlias, "?"));

				foreach (ExpressionListElement arg in invexp.Arguments.Arguments)
				{
					ProcessExpression(nodes, arg.Value, ConcatAlias(expressionAlias, "?"));
				}
				return;
			}

			if (exp is UnaryOperatorExpression)
			{
				var uexp = (UnaryOperatorExpression) exp;
				ProcessExpression(nodes, uexp.Operand, ConcatAlias(expressionAlias, "?"));
				return;
			}

			if (exp is FunctionExpression)
			{
				var fexp = (FunctionExpression) exp;
				ProcessFunctionExpression(nodes, fexp, expressionAlias);
				return;
			}

			if (exp is ObjectLiteralExpression)
			{
				var ojexp = (ObjectLiteralExpression) exp;
				foreach (ObjectLiteralElement element in ojexp.Elements)
				{
					var alias = "?";
					if (element.Name is IdentifierExpression)
					{
						alias = ((IdentifierExpression) element.Name).ID.Spelling;
					}

					ProcessExpression(nodes, element.Value, ConcatAlias(expressionAlias, alias));
				}

				return;
			}
		}

		/// <summary>
		/// The create nodes.
		/// </summary>
		/// <param name="statement">
		/// The statement.
		/// </param>
		/// <param name="nodes">
		/// The nodes.
		/// </param>
		private static void ProcessStatement(Hierachy<CodeNode> nodes, Statement statement)
		{
			if (statement == null)
			{
				return;
			}

			switch (statement.Opcode)
			{
				case Statement.Operation.Expression:
					var exp = (ExpressionStatement) statement;
					ProcessExpression(nodes, exp.Expression, string.Empty);
					break;
				case Statement.Operation.Function:
					{
						var functionStatement = statement as FunctionStatement;
						var node = new CodeNode
						{
							Alias = functionStatement.Function.Name.Spelling,
							Opcode = functionStatement.Opcode.ToString(),
							StartLine = functionStatement.Location.StartLine
						};

						Hierachy<CodeNode> hi = nodes.Add(node);
						CreateNodes(hi, functionStatement.Function.Body.Children);
					}

					break;
				case Statement.Operation.VariableDeclaration:
					var variableDeclarationStatement = statement as VariableDeclarationStatement;
					foreach (VariableDeclarationListElement element in variableDeclarationStatement.Declarations)
					{
						if (element.Declaration is InitializerVariableDeclaration)
						{
							var variableDeclaration = (InitializerVariableDeclaration) element.Declaration;
							var alias = variableDeclaration.Name != null ? variableDeclaration.Name.Spelling : string.Empty;
							ProcessExpression(nodes, variableDeclaration.Initializer, alias);
						}
					}

					break;
			}

			return;
		}

		/// <summary>
		/// The process function expression.
		/// </summary>
		/// <param name="nodes">
		/// The nodes.
		/// </param>
		/// <param name="funcExp">
		/// The func exp.
		/// </param>
		/// <param name="functionName">
		/// The function Name.
		/// </param>
		/// <returns>
		/// Parsed code node.
		/// </returns>
		private static CodeNode ProcessFunctionExpression(
			Hierachy<CodeNode> nodes,
			FunctionExpression funcExp,
			string functionName)
		{
			var name = !string.IsNullOrEmpty(functionName)
				? functionName
				: (funcExp.Function.Name != null
					? funcExp.Function.Name.Spelling
					: string.Empty);

			var codeNode = new CodeNode
			{
				Alias = name,
				Opcode = funcExp.Opcode.ToString(),
				StartLine = funcExp.Location.StartLine
			};

			Hierachy<CodeNode> hi = nodes.Add(codeNode);
			CreateNodes(hi, funcExp.Function.Body.Children);
			return codeNode;
		}
	}
}