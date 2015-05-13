using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;
using JsParser.Core.Code;
using JsParser.Core.Helpers;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace JsParser.Core.Parsers
{
    /// <summary>
    /// The js parser.
    /// </summary>
    public class JavascriptStructureParser
    {
        private CommentsAgregator _comments;
        private JavascriptParserSettings _settings;

        public JavascriptStructureParser(JavascriptParserSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="sourceCode">
        /// The js script.
        /// </param>
        /// <returns>
        /// Hierarhy with code structure.
        /// </returns>
        public JSParserResult Parse(string sourceCode)
        {
            var nodes = new Hierachy<CodeNode>(new CodeNode { Alias = "All" });
            List<ErrorMessage> errors = new List<ErrorMessage>();
            List<ErrorMessage> internalErrors = null;
            List<TaskListItem> taskList = new List<TaskListItem>();

            if (!string.IsNullOrEmpty(sourceCode))
            {
                try
                {
                    var parser = new Parser(sourceCode.ToCharArray(), true);
                    var comments = new List<Comment>();
                    var bindingInfo = new BindingInfo();
                    var sourceElements = parser.ParseProgram(ref comments, ref bindingInfo);

                    errors.AddRange(parser.Diagnostics.Select(d => new ErrorMessage
                    {
                        Message = d.Code.ToString(),
                        StartLine = d.Location.StartLine,
                        StartColumn = d.Location.StartColumn
                    }));

                    _comments = new CommentsAgregator();
                    _comments.ProcessComments(comments.Select(c => new CommentWrapper(c)));

                    CreateNodes(nodes, sourceElements);

                    taskList.AddRange(TaskListAggregator.GetTaskList(_comments.Comments, _settings.ToDoKeyWords));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    var errMessage = new ErrorMessage
                    {
                        Message = "Javascript Parser Internal Error: " + ex.Message,
                        StartLine = 1,
                        StartColumn = 1,
                    };

                    errors.Add(errMessage);

                    internalErrors = internalErrors ?? new List<ErrorMessage>();
                    internalErrors.Add(errMessage);
                }
            }

            var result = new JSParserResult
            {
                Nodes = nodes,
                Errors = errors,
                InternalErrors = internalErrors,
                TaskList = taskList,
                FileName = _settings.Filename,
            };

            return result;
        }

        private void CreateNodes<ElementType, ParentType>(Hierachy<CodeNode> nodes, DList<ElementType, ParentType> statements)
        {
            foreach (var statement in statements.GetEnumerable())
            {
                ProcessStatement(nodes, statement);
            }
        }

        private NodeAlias ProcessExpression(Hierachy<CodeNode> nodes, Expression exp, NodeAlias expressionAlias)
        {
            if (exp == null)
            {
                return null;
            }

            if (exp is BinaryOperatorExpression)
            {
                var bexp = (BinaryOperatorExpression) exp;

                var alias = ProcessExpression(nodes, bexp.Left, expressionAlias);

                var falias = bexp.Opcode == Expression.Operation.Equal ? expressionAlias.Concat(alias) : expressionAlias;

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
                        alias.Text += "(" + args +")";
                    }
                }

                
                foreach (ExpressionListElement arg in invexp.Arguments.Arguments)
                {
                    var argAlias = GetAliasForExpr(arg.Value);
                    var itemAlias = expressionAlias.Concat(alias.Concat(argAlias));

                    ProcessExpression(nodes, arg.Value, itemAlias);
                }

                return alias;
            }

            if (exp is UnaryOperatorExpression)
            {
                var uexp = (UnaryOperatorExpression) exp;
                var opAlias = GetAliasForExpr(uexp.Operand);
                var alias = ProcessExpression(nodes, uexp.Operand, expressionAlias.Concat(opAlias));
                return alias;
            }

            if (exp is FunctionExpression)
            {
                var fexp = (FunctionExpression) exp;
                var alias = ProcessFunctionDefinition(nodes, fexp.Function, expressionAlias);
                return alias;
            }

            if (exp is ObjectLiteralExpression)
            {
                var ojexp = (ObjectLiteralExpression) exp;

                if (expressionAlias == null)
                {
                    expressionAlias = new NodeAlias(string.Empty);
                }

                var codeNode = new CodeNode
                {
                    Alias = expressionAlias.GetFullText(),
                    AliasType = expressionAlias.Type,
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

                return expressionAlias;
            }

            if (exp is QualifiedExpression)
            {
                var qexp = (QualifiedExpression) exp;
                var alias = (qexp.Qualifier != null)
                    ? qexp.Qualifier.Spelling
                    : null;

                var basealias = ProcessExpression(nodes, qexp.Base, expressionAlias);

                return basealias.Concat(new NodeAlias(alias));
            }

            if (exp is IdentifierExpression)
            {
                // Just return the alias.
                var iexp = (IdentifierExpression) exp;
                return new NodeAlias(iexp.ID.Spelling);
            }

            if (exp is StringLiteralExpression)
            {
                var slexp = (StringLiteralExpression) exp;
                return new NodeAlias(slexp.Value);
            }

            if (exp is NumericLiteralExpression)
            {
                var numexp = (NumericLiteralExpression)exp;
                return new NodeAlias(numexp.Spelling);
            }

            if (exp is SubscriptExpression)
            {
                var subexp = (SubscriptExpression)exp;
                var basealias = ProcessExpression(nodes, subexp.Base, expressionAlias);
                var subalias = ProcessExpression(nodes, subexp.Subscript, null);
                return basealias.Concat(subalias);
            }

            //Describes "condition ? true : false" construction
            if (exp is TernaryOperatorExpression)
            {
                var trexp = (TernaryOperatorExpression)exp;
                var alias1 = ProcessExpression(nodes, trexp.First, expressionAlias);
                var alias2 = ProcessExpression(nodes, trexp.Second, expressionAlias);
                var alias3 = ProcessExpression(nodes, trexp.Third, expressionAlias);
                return alias1.Concat(alias2).Concat(alias3);
            }

            //General case
            return new NodeAlias(exp.Opcode.ToString());
        }

        private NodeAlias GetAliasForExpr(Expression expr)
        {
            var aliasStr = "?";
            // Try to find name of the function passed as parameter - name of anonimous function
            if (expr.Opcode == Expression.Operation.Function && expr is FunctionExpression)
            {
                if (((FunctionExpression)expr).Function.Name != null)
                {
                    var fn = ((FunctionExpression)expr).Function.Name.Spelling;
                    if (!string.IsNullOrEmpty(fn))
                    {
                        aliasStr = fn;
                    }
                }
            }

            return new NodeAlias(aliasStr, aliasStr == "?" ? NodeAliasType.Anonymous : NodeAliasType.Unknown);
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
                        var variableDeclaration = (InitializerVariableDeclaration)element.Declaration;
                        var alias = variableDeclaration.Name != null ? variableDeclaration.Name.Spelling : string.Empty;

                        var res = ProcessExpression(nodes, variableDeclaration.Initializer, new NodeAlias(alias));

                        if (!(variableDeclaration.Initializer is FunctionExpression) &&
                            !(variableDeclaration.Initializer is ObjectLiteralExpression))
                        {
                            var varCodeNode = ProcessVariableDefinition(nodes, element.Declaration);
                        }
                    }
                    else
                    {
                        ProcessVariableDefinition(nodes, element.Declaration);
                    }
                }

                return;
            }

            if (statement is ReturnOrThrowStatement)
            {
                var rstat = statement as ReturnOrThrowStatement;
                ProcessExpression(nodes, rstat.Value, new NodeAlias("return"));
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
                ProcessExpression(nodes, sstat.Value, new NodeAlias("switch"));

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

        private CodeNode ProcessVariableDefinition(
            Hierachy<CodeNode> nodes,
            VariableDeclaration variableDeclaration
            )
        {
            var codeNode = new CodeNode
            {
                Alias = variableDeclaration.Name != null ? variableDeclaration.Name.Spelling : "?",
                AliasType = NodeAliasType.Variable,
                Opcode = "Variable",
                StartLine = variableDeclaration.Location.StartLine,
                StartColumn = variableDeclaration.Location.StartColumn,
                EndLine = variableDeclaration.Location.EndLine,
                EndColumn = variableDeclaration.Location.EndColumn,
                Comment = _comments.GetComment(variableDeclaration.Location.StartLine, variableDeclaration.Location.EndLine)
            };

            Hierachy<CodeNode> hi = nodes.Add(codeNode);
            return codeNode;
        }

        private NodeAlias ProcessFunctionDefinition(
            Hierachy<CodeNode> nodes,
            FunctionDefinition function,
            NodeAlias functionName)
        {
            if (functionName == null)
            {
                var name = function.Name != null
                    ? function.Name.Spelling
                    : string.Empty;
                functionName = new NodeAlias(name);
            }
            
            var pars = string.Join(
                ",",
                ((IEnumerable<Parameter>) function.Parameters)
                .Select(p => p.Name != null ? p.Name.Spelling : string.Empty)
                .ToArray());
            pars = pars.Shortenize(_settings.MaxParametersLength);

            var codeNode = new CodeNode
            {
                Alias = functionName.GetFullText() + "(" + pars + ")",
                AliasType = functionName.Type,
                Opcode = Expression.Operation.Function.ToString(),
                StartLine = function.Location.StartLine,
                StartColumn = function.Location.StartColumn,
                EndLine = function.Location.EndLine,
                EndColumn = function.Location.EndColumn,
                Comment = _comments.GetComment(function.Location.StartLine, function.Location.EndLine)
            };

            //Go to recursion
            Hierachy<CodeNode> hi = nodes.Add(codeNode);
            CreateNodes(hi, function.Body.Children);
            return new NodeAlias(codeNode.Alias);
        }
    }
}