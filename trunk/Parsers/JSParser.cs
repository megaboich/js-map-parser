using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;

namespace JS_addin.Addin.Parsers.JSParser
{
	public class JSParser
	{
		public static IEnumerable<CodeNode> Parse(string jsScript)
		{
			Parser parser = new Parser(jsScript.ToCharArray(), true);
			List<Comment> comments = new List<Comment>();
			BindingInfo bindingInfo = new BindingInfo();
			DList<Statement, BlockStatement> sourceElements = parser.ParseProgram(ref comments, ref bindingInfo);

			return CreateNodes(sourceElements);
		}

		public static IList<CodeNode> CreateNodes(DList<Statement, BlockStatement> statements)
		{
			DList<Statement, BlockStatement>.Iterator iterator = new DList<Statement, BlockStatement>.Iterator(statements);
			IList<CodeNode> nodes = new List<CodeNode>();
			while (iterator.Element != null)
			{
				Statement statement = iterator.Element;
				if (statement != null)
				{
					IList<CodeNode> childNodes = CreateNodes(statement);
					if (childNodes != null && childNodes.Count > 0)
					{
						foreach (CodeNode node in childNodes)
						{
							nodes.Add(node);
						}
					}
				}
				iterator.Advance();
			}

			return nodes;
		}

		private static IList<CodeNode> CreateNodes(Statement statement)
		{
			if (statement == null) return null;
			IList<CodeNode> nodes = new List<CodeNode>();
			switch (statement.Opcode)
			{
				case Statement.Operation.Expression:
					//This case when function is declared as member
					//example:
					// this.onError = function(result) { alert(result.message); };
					var exp = (ExpressionStatement)statement;
					if (exp.Expression is BinaryOperatorExpression)
					{
						var bexp = (BinaryOperatorExpression)exp.Expression;
						if (bexp.Right.Opcode == Microsoft.JScript.Compiler.ParseTree.Expression.Operation.Function)
						{
							string alias = "???";
							int startline = exp.Location.StartLine;

							if (bexp.Left is QualifiedExpression)
							{
								alias = ((QualifiedExpression)bexp.Left).Qualifier.Spelling;
							}
							nodes.Add(new CodeNode()
							{
								Alias = alias,
								Opcode = bexp.Right.Opcode.ToString(),
								StartLine = startline
							});
						}
					}
					break;
				case Statement.Operation.Function:
					FunctionStatement functionStatement = statement as FunctionStatement;
					var node = new CodeNode()
					{
						Alias = functionStatement.Function.Name.Spelling,
						Opcode = functionStatement.Opcode.ToString(),
						StartLine = functionStatement.Location.StartLine
					};
					nodes.Add(node);
					var childs = CreateNodes(functionStatement.Function.Body.Children);
					foreach (var child in childs)
					{
						nodes.Add(child);
						child.Alias = node.Alias + "." + child.Alias;
					}
					break;
				case Statement.Operation.VariableDeclaration:
					VariableDeclarationStatement variableDeclarationStatement = statement as VariableDeclarationStatement;
					foreach (VariableDeclarationListElement element in variableDeclarationStatement.Declarations)
					{
						InitializerVariableDeclaration variableDeclaration = element.Declaration as InitializerVariableDeclaration;
						if (variableDeclaration != null)
						{
							if (variableDeclaration.Initializer is FunctionExpression)
							{
								nodes.Add(new CodeNode()
								{
									Alias = variableDeclaration.Name.Spelling,
									Opcode = variableDeclaration.Initializer.Opcode.ToString(),
									StartLine = variableDeclaration.Location.StartLine
								});
							}
						}
					}
					break;
			}
			return nodes;
		}
	}
}
