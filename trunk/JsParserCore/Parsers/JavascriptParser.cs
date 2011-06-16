using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;
using JsParserCore.Code;
using JsParserCore.Helpers;
using System.Reflection;
using System.Diagnostics;

namespace JsParserCore.Parsers
{
	/// <summary>
	/// The js parser.
	/// </summary>
	public class JavascriptParser
	{
		private CommentsAgregator _comments;
		private JavascriptParserSettings _settings;

		public JavascriptParser(JavascriptParserSettings settings)
		{
			_settings = settings;
		}

		/// <summary>
		/// The parse.
		/// </summary>
		/// <param name="script">
		/// The js script.
		/// </param>
		/// <returns>
		/// Hierarhy with code structure.
		/// </returns>
		public Hierachy<CodeNode> Parse(IEnumerable<CodeChunk> codeChunks)
		{
			var nodes = new Hierachy<CodeNode>(new CodeNode { Alias = "All" });
			foreach (var codeChunk in codeChunks)
			{
				try
				{
					var code = codeChunk.Code.Split(new[] { Environment.NewLine, "\r", "\n" }, StringSplitOptions.None);
					var parser = new Parser(codeChunk.Code.ToCharArray(), true);
					var comments = new List<Comment>();
					var bindingInfo = new BindingInfo();
					var sourceElements = parser.ParseProgram(ref comments, ref bindingInfo);
					_comments = new CommentsAgregator(comments, code);
				
					CreateNodes(nodes, sourceElements);
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.ToString());
				}
			}
			return nodes;
		}

		private void CreateNodes<ElementType, ParentType>(Hierachy<CodeNode> nodes, DList<ElementType, ParentType> statements)
		{
			foreach (var statement in statements.GetEnumerable())
			{
				ProcessStatement(nodes, statement);
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

				if (invexp.Arguments.Arguments.Count > 0)
				{
					var args = StringifyArguments(invexp.Arguments.Arguments);
					if (!string.IsNullOrEmpty(args))
					{
						args = args.Shortenize(_settings.MaxParametersLengthInFunctionChain);
						alias += "(" + args +")";
					}
				}

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
						StartLine = exp.Location.StartLine,
						StartColumn = exp.Location.StartColumn,
						EndLine = exp.Location.EndLine,
						EndColumn = exp.Location.EndColumn,
						Comment = _comments.GetComment(exp.Location.StartLine, exp.Location.EndLine)
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

		private string StringifyArguments(List<ExpressionListElement> list)
		{
			return string.Join(",", list.Select(a => StringifyArgument(a.Value)).Where(s => !string.IsNullOrEmpty(s)).ToArray());
		}

		private string StringifyArgument(Expression exp)
		{
			if (exp is IdentifierExpression)
			{
				var iexp = (IdentifierExpression)exp;
				return iexp.ID.Spelling;
			}

			if (exp is StringLiteralExpression)
			{
				var sexp = (StringLiteralExpression)exp;
				return sexp.Spelling;
			}

			if (exp is NumericLiteralExpression)
			{
				var nexp = (NumericLiteralExpression)exp;
				return nexp.Spelling;
			}

			return null;
		}

		private void ProcessStatement<ElementType>(Hierachy<CodeNode> nodes, ElementType statement)
		{
			if (statement == null)
			{
				return;
			}

			if (statement is ExpressionStatement)
			{
				var exp = statement as ExpressionStatement;
				ProcessExpression(nodes, exp.Expression, null);
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
				var rstat = statement as ReturnOrThrowStatement;
				ProcessExpression(nodes, rstat.Value, "return");
				return;
			}

			if (statement is IfStatement)
			{
				var ifstat = statement as IfStatement;
				if (ifstat.IfBody != null)
				{
					ProcessStatement(nodes, ifstat.IfBody);
				}
				if (ifstat.ElseBody != null)
				{
					ProcessStatement(nodes, ifstat.ElseBody);
				}

				return;
			}

			if (statement is BlockStatement)
			{
				var blockstat = statement as BlockStatement;
				CreateNodes(nodes, blockstat.Children);
				return;
			}

			if (statement is SwitchStatement)
			{
				var sstat = statement as SwitchStatement;
				ProcessExpression(nodes, sstat.Value, "switch");

				foreach(var caseClause in sstat.Cases.GetEnumerable())
				{
					CreateNodes(nodes, caseClause.Children);
				}

				return;
			}

			if (statement is TryStatement)
			{
				var tstat = statement as TryStatement;
				if (tstat.Block != null)
				{
					ProcessStatement(nodes, tstat.Block);
				}
				if (tstat.Catch != null)
				{
					ProcessStatement(nodes, tstat.Catch.Handler);
				}
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
				",",
				((IEnumerable<Parameter>) function.Parameters)
				.Select(p => p.Name != null ? p.Name.Spelling : string.Empty)
				.ToArray());
			pars = pars.Shortenize(_settings.MaxParametersLength);

			var codeNode = new CodeNode
			{
				Alias = name + "(" + pars + ")",
				Opcode = Expression.Operation.Function.ToString(),
				StartLine = function.Location.StartLine,
				StartColumn = function.Location.StartColumn,
				EndLine = function.Location.EndLine,
				EndColumn = function.Location.EndColumn,
				Comment = _comments.GetComment(function.Location.StartLine, function.Location.EndLine)
			};

			Hierachy<CodeNode> hi = nodes.Add(codeNode);
			CreateNodes(hi, function.Body.Children);
			return codeNode;
		}
	}
}