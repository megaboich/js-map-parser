using System.Collections.Generic;
using JS_addin.Addin.Code;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;

namespace JS_addin.Addin.Parsers
{
	/// <summary>
	/// The js parser.
	/// </summary>
	public class JavascriptParser
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
			var nodes = new Hierachy<CodeNode>(new CodeNode { Alias = "All" }, null);

			CreateNodes(sourceElements, nodes);
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
		public static void CreateNodes(DList<Statement, BlockStatement> statements, Hierachy<CodeNode> nodes)
		{
			var iterator = new DList<Statement, BlockStatement>.Iterator(statements);
			while (iterator.Element != null)
			{
				Statement statement = iterator.Element;
				if (statement != null)
				{
					CreateNodes(statement, nodes);
				}

				iterator.Advance();
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
		private static void CreateNodes(Statement statement, Hierachy<CodeNode> nodes)
		{
			if (statement == null)
			{
				return;
			}

			switch (statement.Opcode)
			{
				case Statement.Operation.Expression:
					// This case when function is declared as member
					// example:
					// this.onError = function(result) { alert(result.message); };
					var exp = (ExpressionStatement) statement;
					if (exp.Expression is BinaryOperatorExpression)
					{
						var bexp = (BinaryOperatorExpression) exp.Expression;
						if (bexp.Right is FunctionExpression)
						{
							string alias = "???";
							if (bexp.Left is QualifiedExpression)
							{
								alias = ((QualifiedExpression) bexp.Left).Qualifier.Spelling;
							}

							var funcExp = (FunctionExpression) bexp.Right;
							CodeNode codeNode = ProcessFunctionExpression(nodes, funcExp);
							codeNode.Alias = alias;
						}
					}

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
						CreateNodes(functionStatement.Function.Body.Children, hi);
					}

					break;
				case Statement.Operation.VariableDeclaration:
					var variableDeclarationStatement = statement as VariableDeclarationStatement;
					foreach (VariableDeclarationListElement element in variableDeclarationStatement.Declarations)
					{
						var variableDeclaration = element.Declaration as InitializerVariableDeclaration;
						if (variableDeclaration != null && variableDeclaration.Initializer is FunctionExpression)
						{
							var funcExp = (FunctionExpression) variableDeclaration.Initializer;
							ProcessFunctionExpression(nodes, funcExp);
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
		/// <returns>
		/// Parsed code node.
		/// </returns>
		private static CodeNode ProcessFunctionExpression(Hierachy<CodeNode> nodes, FunctionExpression funcExp)
		{
			var codeNode = new CodeNode
			{
				Alias = (funcExp.Function.Name != null) ? funcExp.Function.Name.Spelling : string.Empty,
				Opcode = funcExp.Opcode.ToString(),
				StartLine = funcExp.Location.StartLine
			};

			Hierachy<CodeNode> hi = nodes.Add(codeNode);
			CreateNodes(funcExp.Function.Body.Children, hi);
			return codeNode;
		}
	}
}