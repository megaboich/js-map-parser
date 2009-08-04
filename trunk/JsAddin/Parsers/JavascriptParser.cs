using System;
using System.Collections.Generic;
using System.Linq;
using JS_addin.Addin.Code;
using JS_addin.Addin.Helpers;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;

namespace JS_addin.Addin.Parsers
{
	/// <summary>
	/// The js parser.
	/// </summary>
	public class JavascriptParser
	{
		private List<Comment> _comments;
		private string[] _code;

		/// <summary>
		/// The parse.
		/// </summary>
		/// <param name="script">
		/// The js script.
		/// </param>
		/// <returns>
		/// Hierarhy with code structure.
		/// </returns>
		public Hierachy<CodeNode> Parse(string script)
		{
			_code = script.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

			var parser = new Parser(script.ToCharArray(), true);
			_comments = new List<Comment>();
			var bindingInfo = new BindingInfo();
			DList<Statement, BlockStatement> sourceElements = parser.ParseProgram(ref _comments, ref bindingInfo);
			var nodes = new Hierachy<CodeNode>(new CodeNode { Alias = "All" });

			CreateNodes(nodes, sourceElements);
			return nodes;
		}

		private void CreateNodes(Hierachy<CodeNode> nodes, DList<Statement, BlockStatement> statements)
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

		private string ConcatAlias(string exist, string appender)
		{
			if (string.IsNullOrEmpty(exist))
			{
				return appender;
			}

			return exist + "." + appender;
		}

		private string ProcessExpression(Hierachy<CodeNode> nodes, Expression exp, string expressionAlias)
		{
			if (exp is BinaryOperatorExpression)
			{
				var bexp = (BinaryOperatorExpression) exp;

				string alias = ProcessExpression(nodes, bexp.Left, expressionAlias);

				var falias = bexp.Opcode == Expression.Operation.Equal ? ConcatAlias(expressionAlias, alias) : expressionAlias;

				ProcessExpression(nodes, bexp.Right, falias);
				return alias;
			}

			if (exp is InvocationExpression)
			{
				var invexp = (InvocationExpression) exp;
				var alias = ProcessExpression(nodes, invexp.Target, expressionAlias);

				foreach (ExpressionListElement arg in invexp.Arguments.Arguments)
				{
					ProcessExpression(nodes, arg.Value, ConcatAlias(expressionAlias, ConcatAlias(alias, "?")));
				}

				return alias;
			}

			if (exp is UnaryOperatorExpression)
			{
				var uexp = (UnaryOperatorExpression) exp;
				ProcessExpression(nodes, uexp.Operand, ConcatAlias(expressionAlias, "?"));
				return string.Empty;
			}

			if (exp is FunctionExpression)
			{
				var fexp = (FunctionExpression) exp;
				var alias = ProcessFunctionDefinition(nodes, fexp.Function, expressionAlias);
				return alias.Alias;
			}

			if (exp is ObjectLiteralExpression)
			{
				var ojexp = (ObjectLiteralExpression) exp;

				if (ojexp.Elements.Count > 0)
				{
					var codeNode = new CodeNode
					{
						Alias = expressionAlias,
						Opcode = exp.Opcode.ToString(),
						StartLine = exp.Location.StartLine
					};
					Hierachy<CodeNode> hi = nodes.Add(codeNode);

					foreach (ObjectLiteralElement element in ojexp.Elements)
					{
						var alias = ProcessExpression(hi, element.Name, expressionAlias);
						ProcessExpression(hi, element.Value, alias);
					}
				}

				return expressionAlias;
			}

			if (exp is QualifiedExpression)
			{
				var qexp = (QualifiedExpression) exp;
				var alias = qexp.Qualifier.Spelling;
				var basealias = ProcessExpression(nodes, qexp.Base, expressionAlias);

				return ConcatAlias(basealias, alias);
			}

			if (exp is IdentifierExpression)
			{
				// Just return the alias.
				var iexp = (IdentifierExpression) exp;
				return iexp.ID.Spelling;
			}

			return string.Empty;
		}

		private void ProcessStatement(Hierachy<CodeNode> nodes, Statement statement)
		{
			if (statement == null)
			{
				return;
			}

			if (statement is ExpressionStatement)
			{
				var exp = (ExpressionStatement) statement;
				ProcessExpression(nodes, exp.Expression, string.Empty);
				return;
			}
			
			if (statement is FunctionStatement)
			{
				var functionStatement = statement as FunctionStatement;
				ProcessFunctionDefinition(nodes, functionStatement.Function, null);
				return;
			}

			if (statement is VariableDeclarationStatement)
			{
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

				return;
			}

			if (statement is ReturnOrThrowStatement)
			{
				var rstat = (ReturnOrThrowStatement) statement;
				ProcessExpression(nodes, rstat.Value, "return");
				return;
			}

			if (statement is SwitchStatement)
			{
				var sstat = (SwitchStatement)statement;
				ProcessExpression(nodes, sstat.Value, "switch");

				var iterator = new DList<CaseClause, SwitchStatement>.Iterator(sstat.Cases);
				while (iterator.Element != null)
				{
					CaseClause c = iterator.Element;
					var subiterator = new DList<Statement, CaseClause>.Iterator(c.Children);
					while (subiterator.Element != null)
					{
						var casestat = subiterator.Element;
						if (casestat != null)
						{
							ProcessStatement(nodes, casestat);
						}

						subiterator.Advance();
					}

					iterator.Advance();
				}

				return;
			}

			return;
		}

		private CodeNode ProcessFunctionDefinition(
			Hierachy<CodeNode> nodes,
			FunctionDefinition function,
			string functionName)
		{
			var name = !string.IsNullOrEmpty(functionName)
				? functionName
				: (function.Name != null
					? function.Name.Spelling
					: string.Empty);

			var pars = string.Join(
				", ",
				((IEnumerable<Parameter>) function.Parameters)
				.Select(p => p.Name != null ? p.Name.Spelling : string.Empty)
				.ToArray());

			var location = function.Location;
			var commentStr = new List<string>();
			foreach (Comment comment in _comments)
			{
				// The same line
				if (comment.Location.EndLine == location.StartLine)
				{
					commentStr.Add(comment.Spelling);
					continue;
				}

				// The prev line
				if (comment.Location.EndLine == location.StartLine - 1)
				{
					commentStr.Add(comment.Spelling);
					continue;
				}
			}

			var codeNode = new CodeNode
			{
				Alias = name + "(" + pars + ")",
				Opcode = Expression.Operation.Function.ToString(),
				StartLine = location.StartLine,
				Comment = String.Join(Environment.NewLine, commentStr.ToArray())
			};

			Hierachy<CodeNode> hi = nodes.Add(codeNode);
			CreateNodes(hi, function.Body.Children);
			return codeNode;
		}
	}
}